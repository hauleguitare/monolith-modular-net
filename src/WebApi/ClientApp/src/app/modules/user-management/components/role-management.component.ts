import { AfterViewInit, Component, inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButton, MatIconAnchor, MatIconButton } from '@angular/material/button';
import { MatError, MatFormField, MatLabel, MatSuffix } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatInput } from '@angular/material/input';
import { MatSlideToggle } from '@angular/material/slide-toggle';
import { MatTooltip } from '@angular/material/tooltip';
import { TranslocoPipe } from '@ngneat/transloco';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatMenu, MatMenuContent, MatMenuItem, MatMenuTrigger } from '@angular/material/menu';
import { defer, finalize, of, skipWhile, Subject, switchMap, takeUntil } from 'rxjs';
import { MatCheckbox } from '@angular/material/checkbox';
import { MatDialog } from '@angular/material/dialog';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { NgIf } from '@angular/common';
import { RoleResponse } from '@api/role';
import { RoleManagementService } from '../../../core/role-management/role-management.service';
import { AppActions } from '../../../common';
import {
    RoleManagementRolePopupComponent,
    RoleManagementRolePopupComponentProps,
} from './role-management-role-popup.component';
import { AppPopupResult } from '../../../common/types';
import { User } from '../../../core/user/user.types';
import { UserService } from '../../../core/user/user.service';
import { cloneDeep } from 'lodash-es';


type RoleListElementManagement = RoleResponse & {position: number, name?: string};

@Component({
    selector: 'app-role-management-overview',
    templateUrl: 'role-management.component.html',
    imports: [
        FormsModule,
        MatButton,
        MatError,
        MatFormField,
        MatIcon,
        MatInput,
        MatLabel,
        MatSlideToggle,
        MatSuffix,
        MatTooltip,
        TranslocoPipe,
        MatTableModule,
        MatPaginatorModule,
        MatIconAnchor,
        MatMenuTrigger,
        MatMenuItem,
        MatMenuContent,
        MatMenu,
        MatIconButton,
        MatCheckbox,
        MatProgressSpinner,
        NgIf,
    ],
    standalone: true,
})
export class RoleManagementComponent implements OnInit, OnDestroy, AfterViewInit
{
    @ViewChild(MatPaginator) paginator: MatPaginator;
    displayedColumns: string[] = ['action', 'position', 'priority', 'name', 'isDefault'];
    dataSource = new MatTableDataSource<RoleListElementManagement>([]);
    isLoading: boolean = false;
    private readonly _unsubscribeAll$ = new Subject<void>();
    private _roles: RoleResponse[] = [];
    private _roleManagementService = inject(RoleManagementService);
    private _dialog = inject(MatDialog);
    private _userService = inject(UserService);
    currentUser: User;

    private _transform(role: RoleResponse[]) : RoleListElementManagement[]
    {
        return role.map((role, index) => {
            return {
                position: index + 1,
                ...role,
            }
        })
    }

    getRoles$()
    {
        return defer(() => {
            this.isLoading = true;
            return this._roleManagementService.get();
        })
            .pipe(
                takeUntil(this._unsubscribeAll$),

                finalize(() => this.isLoading = false)
            )
    }

    ngOnInit(): void {
        this.getRoles$()
            .subscribe((roles) => {
                this._roles = [...roles];
                this.dataSource.data = this._transform(roles);
            });


        this._userService.user$
            .pipe(
                takeUntil(this._unsubscribeAll$)
            ).subscribe((user) => this.currentUser = cloneDeep(user))
    }
    ngOnDestroy(): void {
        this._unsubscribeAll$.next();
        this._unsubscribeAll$.complete();
    }

    ngAfterViewInit() {
        this.dataSource.paginator = this.paginator;
    }

    edit(roleId: string) {
        this._dialog.open<RoleManagementRolePopupComponent,
            RoleManagementRolePopupComponentProps, AppPopupResult>(RoleManagementRolePopupComponent, {
            disableClose: false,
            data: {
                title: 'title.roleManagementPopup',
                roleId,
                action: AppActions.EDIT
            }
        })
            .afterClosed()
            .pipe(
                takeUntil(this._unsubscribeAll$),

                switchMap((result) => {
                    if (!result.isDirty) {
                        return of()
                    }
                    return this.getRoles$();
                }),

                skipWhile((pipe) => !pipe)
            ).subscribe((roles) => {
            this._roles = [...roles];
            this.dataSource.data = this._transform(roles);
        });
    }


    checkRolePriority(priority: number)
    {
        if (!this.currentUser)
        {
            return false;
        }

        return this.currentUser.roles.some(e => e.priority < priority);
    }
}
