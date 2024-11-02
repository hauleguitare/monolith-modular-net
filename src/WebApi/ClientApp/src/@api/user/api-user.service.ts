import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../types';
import { UpdateUserRequest, UserResponse } from './types';

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

    update(id: string, user: UpdateUserRequest)
    {
        return this._http.patch<ApiResponse<UserResponse>>(`${this.endpoint}/${id}`, user);
    }
}
