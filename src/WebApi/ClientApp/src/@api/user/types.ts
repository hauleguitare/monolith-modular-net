export interface UserResponse {
    id: string;
    userName: string;
    firstName?: string;
    lastName?: string;
    email: string;
    emailConfirmed: boolean;
    phoneNumber?: string;
    phoneNumberConfirmed: boolean;
    avatarUrl?: string;
}
