import { RoleResponse } from '../role';

export interface UserResponse {
    isActive: boolean;
    id: string;
    userName: string;
    firstName?: string;
    lastName?: string;
    email: string;
    emailConfirmed: boolean;
    phoneNumber?: string;
    phoneNumberConfirmed: boolean;
    avatarUrl?: string;
    roles: RoleResponse[];
}

export interface PatchUpdateUserRequest {
    firstName?: string;
    lastName?: string;
    email: string;
    phoneNumber?: string;
    phoneNumberConfirmed: boolean;
    avatarUrl?: string;
}

export interface UpdateUserRequest {
    firstName?: string;
    lastName?: string;
    email: string;
    phoneNumber?: string;
    phoneNumberConfirmed: boolean;
    avatarUrl?: string;
    roleNames: string[];
}
