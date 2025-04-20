import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterFormService } from './register-form.service';
import { HttpService } from '../../../services/http.service';
import { AuthService } from '../../../auth/auth.service';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    imports: [CommonModule, ReactiveFormsModule]
})
export class RegisterComponent {
    constructor(
        private registerFormService: RegisterFormService,
        private router: Router,
        private httpService: HttpService,
        private authService: AuthService
    ) { }

    ngOnInit() {
        this.registerForm.reset();

        if (this.authService.isLoggedIn()) {
            this.router.navigate(['/patients']);
        }
    }

    get registerForm() {
        return this.registerFormService.getForm();
    }

    onSubmit() {
        if (this.registerForm.valid) {
            this.httpService.post<any>('auth/register', this.registerForm.value).subscribe({
                next: (response: any) => {
                    this.router.navigate(['/login']);
                },
                error: (error: any) => {
                    console.log(error);
                }
            });
        }
    }
}