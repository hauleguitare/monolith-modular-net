import {Injectable} from "@angular/core";
import { stringHelper } from '../../../utils/stringHelper';

@Injectable({
    providedIn: "root"
})
export class LocalStorageService
{
    constructor() {
    }

    get<T>(key: string): T
    {
        let json = localStorage.getItem(key);

        return JSON.parse(json);
    }

    set(key: string, value: any)
    {
        localStorage.setItem(key, JSON.stringify(value));
    }

    remove(key: string)
    {
        localStorage.removeItem(key)
    }

    clear()
    {
        localStorage.clear();
    }

    has(key: string) {
        return !stringHelper.isEmptyOrNull(localStorage.getItem(key));
    }
}
