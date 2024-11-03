import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AddSessionProcessRequest } from './types';
import { ApiResponse } from '../types';
import { CompanyTaskSessionResponse } from '../task/types';

@Injectable({
    providedIn: "root"
})
export class ApiCompanyTaskSessionService
{
    private _http = inject(HttpClient);


    add(request: AddSessionProcessRequest)
    {
        return this._http.post<ApiResponse<CompanyTaskSessionResponse>>(`api/master/task-sessions`, request)
    }
}
