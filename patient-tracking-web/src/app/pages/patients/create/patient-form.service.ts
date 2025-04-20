import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Injectable({
    providedIn: 'root'
})
export class PatientFormService {
    private patientForm: FormGroup;

    constructor(formBuilder: FormBuilder) {
        this.patientForm = formBuilder.group({
            name: ['', Validators.required],
            surname: ['', Validators.required],
            birthDate: ['', Validators.required]
        });
    }

    getForm(): FormGroup {
        return this.patientForm;
    }

    isFormValid(): boolean {
        return this.patientForm.valid;
    }
}
