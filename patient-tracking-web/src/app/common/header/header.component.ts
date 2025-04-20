import { Component } from "@angular/core";
import { TokenService } from "../../services/token.service";
import { Router } from "@angular/router";
import { AuthService } from "../../auth/auth.service";
import { CommonModule } from "@angular/common";

@Component({
    selector: "app-header",
    templateUrl: "./header.component.html",
    imports: [CommonModule]
})
export class HeaderComponent {
    constructor(
        public authService: AuthService,
    ) { }

    get userEmail() {
        return this.authService.isLoggedIn() ? this.authService.getUserEmail() : null;
    }

    onLogout() {
        this.authService.logout();
    }
}