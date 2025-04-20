import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginFormService } from './login-form.service';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../auth/auth.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    imports: [CommonModule, ReactiveFormsModule]
})
export class LoginComponent {
    constructor(
        private loginFormService: LoginFormService,
        private authService: AuthService,
        private router: Router
    ) { }

    ngOnInit() {
        this.loginForm.reset();

        if (this.authService.isLoggedIn()) {
            this.router.navigate(['/patients']);
        }
    }

    get loginForm() {
        return this.loginFormService.getForm();
    }

    onSubmit() {
        if (this.loginForm.valid) {
            this.authService.login(this.loginForm.value);
        }
    }
}
