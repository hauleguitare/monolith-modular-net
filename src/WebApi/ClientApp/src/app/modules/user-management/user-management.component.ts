import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDrawer, MatDrawerContainer, MatDrawerContent, MatDrawerMode } from '@angular/material/sidenav';
import { MatIcon } from '@angular/material/icon';
import { MatIconButton } from '@angular/material/button';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { TranslocoPipe } from '@ngneat/transloco';
import { Subject } from 'rxjs';
import { BooleanInput } from '@angular/cdk/coercion';
import { NgClass } from '@angular/common';

@Component({
    selector: 'app-user-management',
    templateUrl: 'user-management.component.html',
    imports: [
        MatDrawer,
        MatDrawerContainer,
        MatDrawerContent,
        MatIcon,
        MatIconButton,
        RouterOutlet,
        TranslocoPipe,
        RouterLink,
        RouterLinkActive,
        NgClass,
    ],
    standalone: true,
})
export class UserManagementComponent implements OnInit, OnDestroy
{
    private readonly _unsubscribeAll$ = new Subject<void>();
    mode: MatDrawerMode = "side";
    opened: BooleanInput = true;
    catalog: {icon: string, title: string, content: string, path: string}[] = [
        {
            icon: 'heroicons_outline:user-circle',
            title: 'user',
            content: 'Manage users information',
            path: 'overview'
        },
        {
            icon: 'heroicons_outline:user-circle',
            title: 'roles',
            content: 'Manage roles information',
            path: 'roles'
        }
    ]

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
