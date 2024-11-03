import { inject, Injectable } from '@angular/core';
import { ApiUserService, UpdateUserRequest } from '../../../@api/user';
import { map } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class UserManagementService
{
    private readonly _apiUserService = inject(ApiUserService)
    constructor() {
    }


    get() {
        return this._apiUserService.get().pipe(
            map(({result}) => result)
        );
    }

    getById(userId: string)
    {
        return this._apiUserService.getById(userId).pipe(
            map(({result}) => result)
        );
    }

    update(userId: string, request: UpdateUserRequest)
    {
        return this._apiUserService.update(userId, request)
            .pipe(
                map(({result}) => result)
            )
    }

    updateRoles(userId: string, roleNames: string[])
    {
        return this._apiUserService.updateRoles(userId, roleNames)
    }
}
