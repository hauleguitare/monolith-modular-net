import { Component, Inject, inject, OnDestroy, OnInit } from '@angular/core';
import { defer, finalize, Subject, switchMap, takeUntil, tap } from 'rxjs';
import { UserManagementService } from '../../../core/user-management/user-management.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AppActions, AppNotifications } from '../../../common';
import { TranslocoPipe } from '@ngneat/transloco';
import { MatButton, MatIconButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { BasePopup } from '../../../common/components/base-popup';
import { cloneDeep } from 'lodash-es';
import { UserResponse } from '../../../../@api/user';
import { MatError, MatFormField, MatLabel, MatSuffix } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatSlideToggle, MatSlideToggleChange } from '@angular/material/slide-toggle';
import { MatTooltip } from '@angular/material/tooltip';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { NgClass } from '@angular/common';
import { MatExpansionModule } from '@angular/material/expansion';
import { AppNotificationService } from '../../../services/app-notification.service';


export type UserManagementUserPopupComponentProps = {title: string, userId: string, action: AppActions}


@Component({
    selector: 'app-user-management-user-popup',
    templateUrl: 'user-management-user-popup.component.html',
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
        MatExpansionModule
    ],
    standalone: true,
})
export class UserManagementUserPopupComponent extends BasePopup<UserManagementUserPopupComponent> implements OnInit, OnDestroy
{
    properties: {
        title: string
        userId: string,
        action: AppActions
    } = {
        title: 'title.userManagementPopup',
        userId: null,
        action: AppActions.VIEW
    }
    formGroup: UntypedFormGroup;
    private readonly _unsubscribeAll$ = new Subject<void>();
    isLoading: boolean = false;
    private readonly _userManagementService = inject(UserManagementService);
    private readonly _appNotificationService = inject(AppNotificationService);
    private _user: UserResponse;
    _formBuilder = inject(UntypedFormBuilder);

    roleSections: {title: string, description: string, value: string}[]

    constructor(
        dialogRef: MatDialogRef<UserManagementUserPopupComponent>,
        @Inject(MAT_DIALOG_DATA) props: UserManagementUserPopupComponentProps
    ) {
        super(dialogRef);
        this.properties = {...props};

        this.roleSections = [
            {
                title: 'Administrator',
                description: 'Grant administrator role for user',
                value: 'Administrator'
            },
            {
                title: 'Moderator',
                description: 'Grant moderator role for user',
                value: 'Moderator'
            },
            {
                title: 'User',
                description: 'Grant user role for user',
                value: 'User'
            }
        ]
    }


    getUser$(userId: string)
    {
        return defer(() => {
            this.isLoading = true;
            return this._userManagementService.getById(userId);
        })
            .pipe(
                takeUntil(this._unsubscribeAll$),
                finalize(() => this.isLoading = false)
            )
    }

    ngOnInit(): void {

        // Create the form
        this.formGroup = this._formBuilder.group({
            firstName: ['', Validators.required],
            lastName: ['', Validators.required],
            email: ['', [Validators.required, Validators.email]],
            emailConfirmed: [false],
            isActive: [false, Validators.required],
            roleNames: [[], Validators.required]
        });

        this.getUser$(this.properties.userId)
            .pipe(
                takeUntil(this._unsubscribeAll$),
            ).subscribe((user) => {
                this._user = cloneDeep(user);

                this.formGroup.patchValue({
                    firstName: user.firstName,
                    lastName: user.lastName,
                    email: user.email,
                    isActive: user.isActive,
                    roleNames: user.roles.map(e => e.name)
                })
        })

    }
    ngOnDestroy(): void {
        this._unsubscribeAll$.next();
        this._unsubscribeAll$.complete();
    }

    toggleChange(state: MatSlideToggleChange, value: string) {
        const checked = state.checked;

        if (checked) {
            const roleNames = this.formGroup.get('roleNames').value;

            this.formGroup.get('roleNames').patchValue([...roleNames, value]);
        }
        else {
            const roleNames = this.formGroup.get('roleNames').value;

            this.formGroup.get('roleNames').patchValue([...roleNames].filter(e => e !== value))
        }
    }

    hasRoles(value: string) {
        return (this.formGroup.get('roleNames').value as string).includes(value);
    }

    save() {
        this.formGroup.markAllAsTouched();

        if (this.formGroup.invalid)
        {
            return;
        }

        const payload = this.formGroup.value;
        this._userManagementService.update(this._user.id, {...payload})
            .pipe(
                takeUntil(this._unsubscribeAll$),

                switchMap(() => this._appNotificationService.success())
            ).subscribe(action => {
                if (action === AppNotifications.Cancelled) {
                    this.markToClose();
                }
        })
    }
}
