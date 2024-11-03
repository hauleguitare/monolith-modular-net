import { Routes } from '@angular/router';
import { UserManagementComponent } from './user-management.component';

export default [
    {
        path: '',
        component: UserManagementComponent,
        children: [
            {
                path: '',
                pathMatch: 'full',
                redirectTo: 'overview'
            },
            {
                path: 'overview',
                loadComponent: () => import('./components/user-management-overview.component').then(c => c.UserManagementOverviewComponent)
            },
            {
                path: 'roles',
                loadComponent: () => import('./components/role-management.component').then(c => c.RoleManagementComponent)
            }
        ]
    }
] as Routes
