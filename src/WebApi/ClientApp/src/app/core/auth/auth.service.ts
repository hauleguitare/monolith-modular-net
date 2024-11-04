import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { AuthUtils } from 'app/core/auth/auth.utils';
import { UserService } from 'app/core/user/user.service';
import { catchError, Observable, of, switchMap, takeUntil, throwError } from 'rxjs';
import { ApiAuthService } from '@api/auth';
import { LocalStorageService } from '@fuse/services/local-storage/local-storage.service';
import { User } from '../user/user.types';

@Injectable({ providedIn: 'root' })
export class AuthService {
    private _authenticated: boolean = false;
    private _httpClient = inject(HttpClient);
    private _userService = inject(UserService);
    private _apiAuthService = inject(ApiAuthService);
    private _localStorageService = inject(LocalStorageService);

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Setter & getter for access token
     */
    set accessToken(token: string) {
        this._localStorageService.set('accessToken', token);
    }

    get accessToken(): string {
        return this._localStorageService.get('accessToken') ?? '';
    }

    set refreshToken(token: string) {
        this._localStorageService.set('refreshToken', token)
    }

    get refreshToken(): string {
        return this._localStorageService.get('refreshToken') ?? '';
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Forgot password
     *
     * @param email
     */
    forgotPassword(email: string): Observable<any> {
        return this._httpClient.post('api/auth/forgot-password', email);
    }

    /**
     * Reset password
     *
     * @param password
     */
    resetPassword(password: string): Observable<any> {
        return this._httpClient.post('api/auth/reset-password', password);
    }

    /**
     * Sign in
     *
     * @param credentials
     */
    signIn(credentials: { email: string; password: string }) {
        // Throw error, if the user is already logged in
        if (this._authenticated) {
            return throwError('User is already logged in.');
        }

        return this._apiAuthService.signIn(credentials).pipe(
            switchMap((response) => {
                // Store the access token in the local storage
                this.accessToken = response.result.accessToken;
                this.refreshToken = response.result.refreshToken;

                // Set the authenticated flag to true
                this._authenticated = true;

                // Set user data
                this._userService.user = {
                    ...response.result.user,
                    name: `${response.result.user.firstName} ${response.result.user.lastName}`,
                    isActive: response.result.user.isActive,
                    status: 'online',
                };

                return of(response);
            })
        );
    }

    /**
     * Refresh user by using the access token
     */
    refreshUserByUsingToken(): Observable<any> {
        // Sign in using the token
        return this._userService.get().pipe(
            catchError(() =>
                // Return false
                of(false)
            ),
            switchMap((response: User) => {
                // Replace the access token with the new one if it's available on
                // the response object.

                // Set the authenticated flag to true
                this._authenticated = true;

                // Store the user on the user service
                this._userService.user = response;

                // Return true
                return of(true);
            })
        );
    }

    /**
     * Sign out
     */
    signOut(): Observable<any> {
        // Remove the access token from the local storage
        this._localStorageService.remove('accessToken');
        this._localStorageService.remove('user');

        // Set the authenticated flag to false
        this._authenticated = false;

        // Return the observable
        return of(true);
    }

    /**
     * Sign up
     *
     * @param user
     */
    signUp(user: {
        firstName: string;
        lastName: string;
        email: string;
        password: string;
    }): Observable<any> {
        return this._apiAuthService.signUp(user);
    }

    /**
     * Unlock session
     *
     * @param credentials
     */
    unlockSession(credentials: {
        email: string;
        password: string;
    }): Observable<any> {
        return this._httpClient.post('api/auth/unlock-session', credentials);
    }

    /**
     * Check the authentication status
     */
    check(): Observable<boolean> {
        // Check if the user is logged in
        if (this._authenticated) {
            return of(true);
        }

        // Check the access token availability
        if (!this.accessToken) {
            return of(false);
        }

        // Check the access token expire date
        if (AuthUtils.isTokenExpired(this.accessToken)) {

            if (!this.refreshToken)
            {
                return of(false);
            }

            return this.signByRefreshToken(this.refreshToken);
        }

        // If the access token exists, and it didn't expire, sign in using it
        return this.refreshUserByUsingToken();
    }


    signByRefreshToken(refreshToken: string) {
        return this._apiAuthService.refresh(refreshToken)
            .pipe(
                switchMap(({ result }) => {
                    // Store the access token in the local storage
                    this.accessToken = result.accessToken;
                    this.refreshToken = result.refreshToken;

                    // Set the authenticated flag to true
                    this._authenticated = true;

                    // Set user data
                    this._userService.user = {
                        ...result.user,
                        name: `${result.user.firstName} ${result.user.lastName}`,
                        isActive: result.user.isActive,
                        status: 'online',
                    };

                    return of(true);
                }),

                catchError((err) => {
                    return of(false)
                })
            );
    }
}
