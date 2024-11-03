import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../types';
import { PatchUpdateUserRequest, UpdateUserRequest, UserResponse } from './types';

@Injectable({providedIn: 'root'})
export class ApiUserService
{
    endpoint = 'api/users';
    private readonly _http = inject(HttpClient);


    getById(userId: string)
    {
        return this._http.get<ApiResponse<UserResponse>>(`${this.endpoint}/${userId}`)
    }


    getBySelf()
    {
        return this._http.get<ApiResponse<UserResponse>>(`${this.endpoint}/self`)
    }

    updateBySelf(user: PatchUpdateUserRequest)
    {
        return this._http.patch<ApiResponse<UserResponse>>(`${this.endpoint}/self`, user);
    }

    update(id: string, user: UpdateUserRequest)
    {
        return this._http.put<ApiResponse<UserResponse>>(`${this.endpoint}/${id}`, user);
    }

    get() {
        return this._http.get<ApiResponse<UserResponse[]>>(`${this.endpoint}`)
    }

    updateRoles(userId: string, roleNames: string[]) {
        return this._http.post(`${this.endpoint}/${userId}/roles`, {roleNames})
    }
}
