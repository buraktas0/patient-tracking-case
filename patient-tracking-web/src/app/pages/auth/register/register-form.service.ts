import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Injectable({
    providedIn: 'root',
})
export class RegisterFormService {
    private form: FormGroup;

    constructor(
        formBuilder: FormBuilder
    ) {
        this.form = formBuilder.group({
            email: ['', [Validators.required, Validators.email]],
            password: ['', [Validators.required, Validators.minLength(8)]]
        });
    }

    getForm(): FormGroup {
        return this.form;
    }

    isFormValid(): boolean {
        return this.form.valid;
    }

}
