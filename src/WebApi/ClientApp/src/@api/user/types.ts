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

export interface RoleResponse {
    id: string;
    name?: string;
    normalizedName?: string;
    priority: number;
    isDefault: boolean;
    permissions: string[];
}

export interface UpdateUserRequest {
    firstName?: string;
    lastName?: string;
    email: string;
    phoneNumber?: string;
    phoneNumberConfirmed: boolean;
    avatarUrl?: string;
}
