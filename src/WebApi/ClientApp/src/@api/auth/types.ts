export interface SignUpRequest {
    email: string;
    password: string;
}

export interface SignInRequest {
    email: string;
    password: string;
}

export interface LoggedInResponse {
    accessToken: string;
    refreshToken: string;
}
