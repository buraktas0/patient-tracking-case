import { Injectable } from '@angular/core';
import { jwtDecode } from "jwt-decode";

@Injectable({
    providedIn: 'root'
})
export class TokenService {
    private accessTokenKey = 'access_token';
    private refreshTokenKey = 'refresh_token';

    constructor() { }

    setTokens(authResult: any) {
        localStorage.setItem(this.accessTokenKey, authResult.accessToken);
        localStorage.setItem(this.refreshTokenKey, authResult.refreshToken);
    }

    removeTokens() {
        localStorage.removeItem(this.accessTokenKey);
        localStorage.removeItem(this.refreshTokenKey);
    }

    getToken() {
        return localStorage.getItem(this.accessTokenKey);
    }

    getRefreshToken() {
        return localStorage.getItem(this.refreshTokenKey);
    }

    isTokenExpired(): boolean {
        const token = this.getToken();
        if (!token) return true;

        try {
            const decoded: any = jwtDecode(token);
            const now = Math.floor(Date.now() / 1000);
            return decoded.exp < now;
        } catch (error) {
            return true;
        }
    }
}
