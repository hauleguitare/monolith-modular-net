import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../types';
import { RoleResponse, UpdateRoleRequest } from './types';

@Injectable({
    providedIn: 'root'
})
export class ApiRoleService
{
    endpoint = "api/roles"
    private _http = inject(HttpClient);


    get() {
        return this._http.get<ApiResponse<RoleResponse[]>>(`${this.endpoint}`)
    }

    getById(roleId: string)
    {
        return this._http.get<ApiResponse<RoleResponse>>(`${this.endpoint}/${roleId}`)
    }

    update(roleId: string, request: UpdateRoleRequest)
    {
        return this._http.put<ApiResponse<RoleResponse>>(`${this.endpoint}/${roleId}`, request);
    }
}
