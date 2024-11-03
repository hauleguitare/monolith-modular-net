import { Component, Inject, inject, OnDestroy, OnInit } from '@angular/core';
import { defer, finalize, Subject, switchMap, takeUntil } from 'rxjs';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AppActions, AppNotifications } from '../../../common';
import { TranslocoPipe } from '@ngneat/transloco';
import { MatButton, MatIconButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { BasePopup } from '../../../common/components/base-popup';
import { cloneDeep } from 'lodash-es';
import { MatError, MatFormField, MatLabel, MatSuffix } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatSlideToggle, MatSlideToggleChange } from '@angular/material/slide-toggle';
import { MatTooltip } from '@angular/material/tooltip';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { NgClass } from '@angular/common';
import { MatExpansionModule } from '@angular/material/expansion';
import { AppNotificationService } from '../../../services/app-notification.service';
import { RoleManagementService } from '../../../core/role-management/role-management.service';
import { RoleResponse } from '../../../../@api/role';
import { MatTabsModule } from '@angular/material/tabs';


export type RoleManagementRolePopupComponentProps = {title: string, roleId: string, action: AppActions}


@Component({
    selector: 'app-user-management-role-popup',
    templateUrl: 'role-management-role-popup.component.html',
    imports: [
        TranslocoPipe,
        MatIconButton,
        MatIcon,
        MatButton,
        MatError,
        MatFormField,
        MatInput,
        MatLabel,
        MatSlideToggle,
        MatSuffix,
        MatTooltip,
        ReactiveFormsModule,
        NgClass,
        MatTabsModule
    ],
    standalone: true,
})
export class RoleManagementRolePopupComponent extends BasePopup<RoleManagementRolePopupComponent> implements OnInit, OnDestroy
{
    properties: {
        title: string
        roleId: string,
        action: AppActions
    } = {
        title: 'title.roleManagementPopup',
        roleId: null,
        action: AppActions.VIEW
    }
    formGroup: UntypedFormGroup;
    private readonly _unsubscribeAll$ = new Subject<void>();
    isLoading: boolean = false;
    private readonly _roleManagementService = inject(RoleManagementService);
    private readonly _appNotificationService = inject(AppNotificationService);
    private _role: RoleResponse;
    _formBuilder = inject(UntypedFormBuilder);


    permissionSection: {
        user: {title: string, description: string, value: string}[]
        roles: {title: string, description: string, value: string}[]
    } = {
        user: [],
        roles: []
    }

    constructor(
        dialogRef: MatDialogRef<RoleManagementRolePopupComponent>,
        @Inject(MAT_DIALOG_DATA) props: RoleManagementRolePopupComponentProps
    ) {
        super(dialogRef);
        this.properties = {...props};

        this.permissionSection.user = [
            {
                title: 'View All',
                description: 'View all users',
                value: 'user:view_all'
            },
            {
                title: 'Update User',
                description: 'Update user',
                value: 'user:update'
            },
            {
                title: 'Delete User',
                description: 'Delete user',
                value: 'user:delete'
            },
            {
                title: 'Set Roles for User',
                description: 'Set roles for user',
                value: 'user:set_roles'
            }
        ]
        this.permissionSection.roles = [
            {
                title: 'View All',
                description: 'View all roles',
                value: 'role:view_all'
            },
            {
                title: 'Create Role',
                description: 'Create new role',
                value: 'role:create'
            },
            {
                title: 'Update Role',
                description: 'Update role',
                value: 'role:update'
            },
            {
                title: 'Delete Role',
                description: 'Delete role',
                value: 'role:delete'
            },
            {
                title: 'Set permissions for Role',
                description: 'Set permissions for role',
                value: 'role:set_permissions'
            }
        ]
    }


    getRole$(roleId: string)
    {
        return defer(() => {
            this.isLoading = true;
            return this._roleManagementService.getById(roleId);
        })
            .pipe(
                takeUntil(this._unsubscribeAll$),
                finalize(() => this.isLoading = false)
            )
    }

    ngOnInit(): void {

        // Create the form
        this.formGroup = this._formBuilder.group({
            name: ['', Validators.required],
            priority: ['', Validators.required],
            isDefault: ['', [Validators.required, Validators.email]],
            permissions: [[], Validators.required]
        });
        this.formGroup.get('isDefault').disable();

        this.getRole$(this.properties.roleId)
            .pipe(
                takeUntil(this._unsubscribeAll$),
            ).subscribe((role) => {
            this._role = cloneDeep(role);
            this.formGroup.patchValue({
                name: role.name,
                priority: role.priority,
                isDefault: role.isDefault,
                permissions: role.permissions
            });

        })

    }
    ngOnDestroy(): void {
        this._unsubscribeAll$.next();
        this._unsubscribeAll$.complete();
    }

    toggleChange(state: MatSlideToggleChange, value: string) {
        const checked = state.checked;

        if (checked) {
            const permissions = this.formGroup.get('permissions').value;

            this.formGroup.get('permissions').patchValue([...permissions, value]);
        }
        else {
            const permissions = this.formGroup.get('permissions').value;

            this.formGroup.get('permissions').patchValue([...permissions].filter(e => e !== value))
        }
    }

    save() {
        this.formGroup.markAllAsTouched();

        if (this.formGroup.invalid)
        {
            return;
        }

        const payload = this.formGroup.getRawValue();

        this._roleManagementService.update(this._role.id, {...payload})
            .pipe(
                takeUntil(this._unsubscribeAll$),

                switchMap(() => this._appNotificationService.success())
            ).subscribe(action => {
            if (action === AppNotifications.Cancelled) {
                this.markToClose();
            }
        })
    }

    hasPermission(value: string) {
        return (this.formGroup.get('permissions').value as string).includes(value);
    }
}
