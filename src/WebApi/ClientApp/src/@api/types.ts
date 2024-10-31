export interface ApiErrorResponse {
    code?: string;
    description?: string;
}

export interface ApiResponse<T = null | unknown | any> {
    result?: T;
    isSucceed: boolean;
    errors?: ApiErrorResponse[];
}
