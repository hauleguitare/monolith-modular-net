import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../types';
import { CompanyTaskResponse } from './types';

@Injectable({
    providedIn: 'root'
})
export class ApiCompanyTaskService
{
    endpoint = "api/master/tasks"
    private _http = inject(HttpClient);


    get() {
        return this._http.get<ApiResponse<CompanyTaskResponse[]>>(`${this.endpoint}`)
    }

    checkSessions(companyTaskId: number, request: {email: string})
    {
        return this._http.post<ApiResponse<boolean>>(`${this.endpoint}/${companyTaskId}/check-sessions`, request);
    }
}
