import { Routes } from '@angular/router';
import { UserSettingsComponent } from './user-settings.component';

export default [
    {
        path: '',
        component: UserSettingsComponent,
        children: [
            {
                path: '',
                pathMatch: 'full',
                redirectTo: 'account'
            },
            {
                path: 'account',
                loadComponent: () => import('./components/user-settings-account.component').then(m => m.UserSettingsAccountComponent)
            }
        ]
    }
] as Routes
