export interface LoginUser {
    userName: string;
    password: string;
}

export interface LoginResponse {
    error: boolean;
    message: string;
    data: { token: string, expired: string }
}