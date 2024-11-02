export interface User {
    isActive: boolean;
    id: string;
    name: string;
    firstName?: string;
    lastName?: string;
    userName: string;
    email: string;
    emailConfirmed: boolean;
    phoneNumber?: string;
    phoneNumberConfirmed: boolean;
    avatarUrl?: string;
    status?: string;
}
