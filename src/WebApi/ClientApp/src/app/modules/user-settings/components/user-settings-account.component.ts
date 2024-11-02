import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { distinctUntilChanged, skipWhile, Subject, takeUntil, takeWhile } from 'rxjs';
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

    _formBuilder = inject(UntypedFormBuilder);
    _userService = inject(UserService);

    ngOnInit(): void {
        // Create the form
        this.formGroup = this._formBuilder.group({
            id       : ['', Validators.required],
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
                    id: user.id,
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


        this._userService.update({...this.formGroup.value})
            .pipe(
                takeUntil(this._unsubscribeAll$)
            ).subscribe(() => {
                console.log("success");
        })

    }
}
