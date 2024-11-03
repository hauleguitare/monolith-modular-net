import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { MatDialogClose } from '@angular/material/dialog';
import { MatButton, MatIconButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatDrawer, MatDrawerContainer, MatDrawerContent, MatDrawerMode, MatSidenav } from '@angular/material/sidenav';
import { TranslocoPipe } from '@ngneat/transloco';
import { BooleanInput } from '@angular/cdk/coercion';
import { RouterOutlet } from '@angular/router';

@Component({
    selector: 'app-user-settings',
    templateUrl: 'user-settings.component.html',
    imports: [
        MatButton,
        MatIcon,
        MatIconButton,
        MatDialogClose,
        TranslocoPipe,
        MatDrawer,
        MatDrawerContainer,
        MatDrawerContent,
        RouterOutlet,
    ],
    standalone: true,
})
export class UserSettingsComponent implements OnInit, OnDestroy
{
    private readonly _unsubscribeAll$ = new Subject<void>();
    mode: MatDrawerMode = "side";
    opened: BooleanInput = true;
    catalog: {icon: string, title: string, content: string, path: string}[] = [{
        icon: 'heroicons_outline:user-circle',
        title: 'nav.account',
        content: 'Manage your public profile and private information',
        path: 'account'
    }]

    constructor(
    ) {
    }

    ngOnInit(): void {
    }
    ngOnDestroy(): void {
        this._unsubscribeAll$.next();
        this._unsubscribeAll$.complete();
    }

}
