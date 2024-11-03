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
import { UserManagementService } from '../../../core/user-management/user-management.service';
import { UserResponse } from '../../../../@api/user';
import { defer, finalize, of, skipWhile, Subject, switchMap, takeUntil } from 'rxjs';
import { MatCheckbox } from '@angular/material/checkbox';
import { MatDialog } from '@angular/material/dialog';
import {
    UserManagementUserPopupComponent,
    UserManagementUserPopupComponentProps,
} from './user-management-user-popup.component';
import { AppActions } from '../../../common';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { NgIf } from '@angular/common';
import { AppPopupResult } from '../../../common/types';


type UserListElementManagement = UserResponse & {position: number, name?: string};

@Component({
    selector: 'app-user-management-overview',
    templateUrl: 'user-management-overview.component.html',
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
export class UserManagementOverviewComponent implements OnInit, OnDestroy, AfterViewInit
{
    @ViewChild(MatPaginator) paginator: MatPaginator;
    displayedColumns: string[] = ['action', 'position', 'email', 'name', 'isActive'];
    dataSource = new MatTableDataSource<UserListElementManagement>([]);
    isLoading: boolean = false;
    private readonly _unsubscribeAll$ = new Subject<void>();
    private _users: UserResponse[] = [];
    private _userManagementService = inject(UserManagementService);
    private _dialog = inject(MatDialog)

    private _transform(user: UserResponse[]) : UserListElementManagement[]
    {
        return user.map((user, index) => {
            return {
                position: index + 1,
                ...user,
                name: `${user.firstName} ${user.lastName}`
            }
        })
    }

    getUsers$()
    {
        return defer(() => {
            this.isLoading = true;
            return this._userManagementService.get();
        })
            .pipe(
                takeUntil(this._unsubscribeAll$),

                finalize(() => this.isLoading = false)
            )
    }

    ngOnInit(): void {
        this.getUsers$()
            .subscribe((users) => {
                this._users = [...users];
                this.dataSource.data = this._transform(users);
            });
    }
    ngOnDestroy(): void {
        this._unsubscribeAll$.next();
        this._unsubscribeAll$.complete();
    }

    ngAfterViewInit() {
        this.dataSource.paginator = this.paginator;
    }

    edit(userId: string) {
        this._dialog.open<UserManagementUserPopupComponent,
            UserManagementUserPopupComponentProps, AppPopupResult>(UserManagementUserPopupComponent, {
                disableClose: false,
                data: {
                    title: 'title.userManagementPopup',
                    userId,
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
                    return this.getUsers$();
                }),

                skipWhile((pipe) => !pipe)
            ).subscribe((users) => {
                this._users = [...users];
                this.dataSource.data = this._transform(users);
        });
    }
}
