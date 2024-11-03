import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { defer, distinctUntilChanged, finalize, skipWhile, Subject, switchMap, takeUntil, takeWhile } from 'rxjs';
import { TranslocoPipe } from '@ngneat/transloco';
import { MatError, MatFormField, MatLabel, MatSuffix } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MatCheckbox } from '@angular/material/checkbox';
import { MatIcon } from '@angular/material/icon';
import { MatTooltip } from '@angular/material/tooltip';
import { NgClass } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatButtonToggle, MatButtonToggleGroup } from '@angular/material/button-toggle';
import { MatSlideToggle } from '@angular/material/slide-toggle';
import { UserService } from '../../../core/user/user.service';
import { User } from '../../../core/user/user.types';
import { cloneDeep } from 'lodash-es';
import { AppNotificationService } from '../../../services/app-notification.service';

@Component({
    selector: 'app-user-settings-account',
    templateUrl: 'user-settings-account.component.html',
    imports: [
        TranslocoPipe,
        MatFormField,
        MatLabel,
        MatInput,
        MatError,
        ReactiveFormsModule,
        MatCheckbox,
        MatIcon,
        MatTooltip,
        NgClass,
        MatSuffix,
        MatButton,
        MatButtonToggle,
        MatButtonToggleGroup,
        MatSlideToggle,
    ],
    standalone: true,
})
export class UserSettingsAccountComponent implements OnInit, OnDestroy
{
    private readonly _unsubscribeAll$ = new Subject<void>();
    formGroup: UntypedFormGroup;
    private _defaultUser: User;
    private _appNotificationService = inject(AppNotificationService)
    isLoading: boolean = false;

    _formBuilder = inject(UntypedFormBuilder);
    _userService = inject(UserService);

    ngOnInit(): void {
        // Create the form
        this.formGroup = this._formBuilder.group({
            firstName: ['', Validators.required],
            lastName: ['', Validators.required],
            email: ['', [Validators.required, Validators.email]],
            emailConfirmed: [false],
            isActive: [false, Validators.required]
        });


        this._userService.get()
            .pipe(
                takeUntil(this._unsubscribeAll$)
            ).subscribe((user) => {
                this.formGroup.patchValue({
                    firstName: user.firstName,
                    lastName: user.lastName,
                    email: user.email,
                    emailConfirmed: user.emailConfirmed,
                    isActive: user.isActive
                });

                this._defaultUser = cloneDeep({...user});
        });
    }
    ngOnDestroy(): void {
        this._unsubscribeAll$.next();
        this._unsubscribeAll$.complete();
    }

    submit() {
        this.formGroup.markAllAsTouched();

        if (this.formGroup.invalid)
        {
            return;
        }


        defer(() => {
            this.isLoading = true;
            return this._userService.update({...this.formGroup.value});
        })
            .pipe(
                takeUntil(this._unsubscribeAll$),

                switchMap(() => this._appNotificationService.success()),

                finalize(() => this.isLoading = false)
            ).subscribe()

    }
}
