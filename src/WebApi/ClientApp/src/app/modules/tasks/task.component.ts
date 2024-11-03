import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { defer, finalize, map, Subject, switchMap, takeUntil } from 'rxjs';
import { MatIcon } from '@angular/material/icon';
import { ApiCompanyTaskService } from '../../../@api/task/api-company-task.service';
import { CompanyTaskResponse } from '../../../@api/task/types';
import { TranslocoPipe } from '@ngneat/transloco';
import { NgStyle } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { TaskSessionPopupComponent, TaskSessionPopupComponentProps } from './components/task-session-popup.component';
import { AppPopupResult } from '../../common/types';

@Component({
    selector: 'app-task',
    templateUrl: 'task.component.html',
    imports: [
        MatIcon,
        TranslocoPipe,
        NgStyle,
        MatButton,
    ],
    standalone: true,
})
export class TaskComponent implements OnInit, OnDestroy
{
    tasks: CompanyTaskResponse[] = [];
    isLoading: boolean = false;
    private readonly _unsubscribeAll$ = new Subject<void>();
    private _apiCompanyTaskService = inject(ApiCompanyTaskService);
    private _dialog = inject(MatDialog);


    getData$() {
        return defer(() => {
            this.isLoading = true;
            return this._apiCompanyTaskService.get();
        })
            .pipe(
                takeUntil(this._unsubscribeAll$),

                map(({ result }) => result),

                finalize(() => this.isLoading = false)
            )
    }
    ngOnInit(): void {
        this.getData$()
            .subscribe((data) => {
                this.tasks = [...data];
            })
    }
    ngOnDestroy(): void {
        this._unsubscribeAll$.next();
        this._unsubscribeAll$.complete();
    }


    getTaskSession(task: CompanyTaskResponse)
    {
        if (!task.sessions.length)
        {
            return null;
        }
        return task.sessions.at(0);
    }

    startSession(id: number) {
        this._dialog.open<TaskSessionPopupComponent, TaskSessionPopupComponentProps, AppPopupResult>(TaskSessionPopupComponent, {
            disableClose: false,
            data: {
                title: 'nav.taskSession',
                companyTaskId: id
            }
        })
    }
}
