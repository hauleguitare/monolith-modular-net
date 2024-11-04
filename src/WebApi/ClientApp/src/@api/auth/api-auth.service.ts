import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoggedInResponse, SignUpRequest } from './types';
import { ApiResponse } from '../types';

@Injectable({
    providedIn: 'root'
})
export class ApiAuthService
{
    endpoint = 'api/auth';
    private readonly _http = inject(HttpClient);

    signUp(body: SignUpRequest)
    {
        return this._http.post<ApiResponse>(`${this.endpoint}/sign-up`, body);
    }

    signIn(body: SignUpRequest)
    {
        return this._http.post<ApiResponse<LoggedInResponse>>(`${this.endpoint}/sign-in`, body);
    }

    logOut()
    {
        return this._http.post<ApiResponse>(`${this.endpoint}/log-out`, {});
    }

    refresh(refreshToken: string) {
        return this._http.post<ApiResponse<LoggedInResponse>>(`${this.endpoint}/refresh`, {refreshToken})
    }
}
