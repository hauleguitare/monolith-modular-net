import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { User } from 'app/core/user/user.types';
import { map, Observable, ReplaySubject, tap } from 'rxjs';
import { LocalStorageService } from '@fuse/services/local-storage/local-storage.service';
import { ApiUserService } from '@api/user';
import { cloneDeep } from 'lodash-es';

@Injectable({ providedIn: 'root' })
export class UserService {
    private _httpClient = inject(HttpClient);
    private _user: ReplaySubject<User> = new ReplaySubject<User>(1);
    private _localStorageService = inject(LocalStorageService);
    private _apiUserService = inject(ApiUserService);
    private _currentUser?: User;

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------
    /**
     * Setter & getter for user
     *
     * @param value
     */
    set user(value: User) {
        // Store the value
        this._user.next(value);
    }

    get user$(): Observable<User> {
        return this._user.asObservable();
    }

    get user() {
        return this._currentUser;
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------


    /**
     * Get the current signed-in user data
     */
    get(): Observable<User> {
        return this._apiUserService.getBySelf().pipe(
            map(({result}) => {
                return {
                    ...result,
                    name: `${result.firstName} ${result.lastName}`,
                    status: 'online',
                } as User
            }),

            tap((user) => {
                this._currentUser = cloneDeep(user);
                this._user.next(user);
            })
        );
    }

    /**
     * Getter for user cached
     * */
    getCached()
    {
        return this._localStorageService.get<User>("user");
    }

    /**
     * Setter for user cached
     * */
    setCached(user: User)
    {
        this._localStorageService.set("user", user);
    }

    /**
     * Getter for check user cached
     * */
    hasCached()
    {
        return this._localStorageService.has("user");
    }

    /**
     * Update the user
     *
     * @param user
     */
    update(user: User): Observable<any> {
        return this._apiUserService.updateBySelf({...user}).pipe(
            map(({ result }) => {
                this._user.next({
                    ...result,
                    name: `${result.firstName} ${result.lastName}`,
                    isActive: result.isActive,
                    status: 'online',
                });
            })
        );
    }
}
