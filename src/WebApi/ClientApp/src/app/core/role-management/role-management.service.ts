import { ApiRoleService, UpdateRoleRequest } from '../../../@api/role';
import { inject, Injectable } from '@angular/core';
import { map } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class RoleManagementService
{
    private readonly _apiRoleService = inject(ApiRoleService)
    constructor() {
    }


    get() {
        return this._apiRoleService.get().pipe(
            map(({result}) => result)
        );
    }

    getById(userId: string)
    {
        return this._apiRoleService.getById(userId).pipe(
            map(({result}) => result)
        );
    }

    update(roleId: string, request: UpdateRoleRequest)
    {
        return this._apiRoleService.update(roleId, request)
            .pipe(
                map(({result}) => result)
            )
    }
}
