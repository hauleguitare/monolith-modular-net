import { Routes } from '@angular/router';

export default [
    {
        path: '',
        children: [
            {
                path: '',
                loadComponent: () => import('./task.component').then(c => c.TaskComponent)
            }
        ]
    }
] as Routes
