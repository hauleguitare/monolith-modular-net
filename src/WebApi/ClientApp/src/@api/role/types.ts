export interface RoleResponse {
    id: string;
    name?: string;
    normalizedName?: string;
    priority: number;
    isDefault: boolean;
    permissions: string[];
}


export interface UpdateRoleRequest {
    id: string;
    name?: string;
    normalizedName?: string;
    priority: number;
    isDefault: boolean;
    permissions: string[];
}
