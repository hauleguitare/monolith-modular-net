import { Component, Inject, inject, OnDestroy, OnInit } from '@angular/core';
import { distinctUntilChanged, of, skipWhile, Subject, switchMap, takeUntil, takeWhile, tap } from 'rxjs';
import { BasePopup } from '../../../common/components/base-popup';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatButton, MatIconButton } from '@angular/material/button';
import { MatError, MatFormField, MatLabel, MatSuffix } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatInput } from '@angular/material/input';
import { MatSlideToggle } from '@angular/material/slide-toggle';
import { MatTooltip } from '@angular/material/tooltip';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { TranslocoPipe } from '@ngneat/transloco';
import { cloneDeep } from 'lodash-es';
import { ApiCompanyTaskService } from '../../../../@api/task/api-company-task.service';
import { ApiCompanyTaskSessionService } from '../../../../@api/task-sessions/api-company-task-session.service';
import { DateTime } from 'luxon';
import { AppNotificationService } from '../../../services/app-notification.service';
import { AppNotifications } from '../../../common';


export type TaskSessionPopupComponentProps = {
    title: string;
    companyTaskId: number;
}

@Component({
    selector: 'app-task-session-popup',
    templateUrl: 'task-session-popup.component.html',
    imports: [
        MatButton,
        MatError,
        MatFormField,
        MatIcon,
        MatIconButton,
        MatInput,
        MatLabel,
        MatSlideToggle,
        MatSuffix,
        MatTooltip,
        ReactiveFormsModule,
        TranslocoPipe,
    ],
    standalone: true,
})
export class TaskSessionPopupComponent extends BasePopup<TaskSessionPopupComponent> implements OnInit, OnDestroy
{
    private readonly _unsubscribeAll$ = new Subject<void>();
    isLoading: boolean = false;
    properties: TaskSessionPopupComponentProps;
    formGroup: UntypedFormGroup;
    private _apiCompanyTaskService = inject(ApiCompanyTaskService);
    private _apiCompanyTaskSessionService = inject(ApiCompanyTaskSessionService);
    private _appNotificationService = inject(AppNotificationService);

    private _formBuilder = inject(UntypedFormBuilder);
    valid: boolean = false;

    constructor(
        dialog: MatDialogRef<TaskSessionPopupComponent>,
        @Inject(MAT_DIALOG_DATA) props: TaskSessionPopupComponentProps
    ) {
        super(dialog);

        this.properties = cloneDeep(props);
    }

    ngOnInit(): void {
        this.formGroup = this._formBuilder.group({
            email: ['', Validators.email],
            password: ['']
        });

    }
    ngOnDestroy(): void {
        this._unsubscribeAll$.next();
        this._unsubscribeAll$.complete();
    }

    save() {
        this._apiCompanyTaskSessionService.add({
            email: this.formGroup.get('email').value,
            statusId: 1,
            companyTaskId: this.properties.companyTaskId
        }).pipe(
            takeUntil(this._unsubscribeAll$),

            switchMap(() => this._appNotificationService.success())
        ).subscribe((action) => {
            if (action == AppNotifications.Cancelled) {
            }
        })
    }

    onFocusOut($event: FocusEvent) {
        if (this.formGroup.get('email').valid) {
            this._apiCompanyTaskService.checkSessions(this.properties.companyTaskId, {email: this.formGroup.get('email').value})
                .pipe(
                    takeUntil(this._unsubscribeAll$)
                ).subscribe(({result}) => this.valid = result);
        }
    }
}
