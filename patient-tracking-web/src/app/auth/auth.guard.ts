import { Injectable } from '@angular/core';
import { TokenService } from '../services/token.service';
import { Router } from '@angular/router';

@Injectable({
    providedIn: 'root'
})
export class AuthGuard {
    constructor(
        private tokenService: TokenService,
        private router: Router
    ) { }

    canActivate(): boolean {
        if (!this.tokenService.getToken() || this.tokenService.isTokenExpired()) {        
            this.tokenService.removeTokens();

            this.router.navigate(['/login']);
            return false;
        }
        return true;
    }
}