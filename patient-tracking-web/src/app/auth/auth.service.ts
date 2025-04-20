import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { HttpService } from "../services/http.service";
import { TokenService } from "../services/token.service";

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    constructor(
        private router: Router,
        private httpService: HttpService,
        private tokenService: TokenService
    ) { }

    login(request: any) {
        this.httpService.post('auth/login', request).subscribe({
            next: (response: any) => {
                this.tokenService.setTokens(response);
                localStorage.setItem('userEmail', request.email);
                this.router.navigate(['/patients']);
            },
            error: (error: any) => {
                console.log(error);
            }
        });
    }

    logout() {
        this.httpService.post('auth/logout', { token: this.tokenService.getRefreshToken() }, true).subscribe({
            next: (response: any) => {
                console.log('Logout successful');

                this.tokenService.removeTokens();
                localStorage.removeItem('userEmail');
                this.router.navigate(['/login']);
            },
            error: (error: any) => {
                console.log('Logout error', error);
            }
        });
    }

    refreshToken() {
        this.httpService.post('auth/refresh-token', { token: this.tokenService.getRefreshToken() }, true).subscribe({
            next: (response: any) => {
                this.tokenService.setTokens(response);
            },
            error: (error: any) => {
                console.log(error);
            }
        });
    }

    getUserEmail() {
        return localStorage.getItem('userEmail');
    }

    isLoggedIn(): boolean {
        return !!this.tokenService.getToken() && !this.tokenService.isTokenExpired();
    }
}   