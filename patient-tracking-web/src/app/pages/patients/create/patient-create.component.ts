import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { PatientFormService } from './patient-form.service';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpService } from '../../../services/http.service';
import { Patient, PatientCreateRequest, PatientUpdateRequest } from '../../../models/patient.model';

@Component({
    selector: 'app-patient-create',
    templateUrl: './patient-create.component.html',
    imports: [CommonModule, ReactiveFormsModule]
})
export class PatientCreateComponent {
    patientId?: number;

    constructor(
        private patientFormService: PatientFormService,
        private router: Router,
        private route: ActivatedRoute,
        private httpService: HttpService
    ) { }

    get patientForm() {
        return this.patientFormService.getForm();
    }

    ngOnInit() {
        this.patientForm.reset();
        this.patientId = this.route.snapshot.params['id'];
        if (this.patientId) {
            this.fetchPatient(this.patientId);
        }
    }

    fetchPatient(patientId: number) {
        console.log('fetchPatient', patientId);
        this.httpService.get(`patient/${patientId}`, true).subscribe({
            next: (response: any) => {
                console.log('Patient fetched successfully', response);
                this.patientForm.patchValue(response);
            },
            error: (error: any) => {
                console.error('Error fetching patient', error);
            }
        });
    }

    onCancel() {
        this.router.navigate(['/patients']);
    }

    onSubmit() {
        if (this.patientForm.valid) {
            if (this.patientId) {
                let patientUpdateRequest = new PatientUpdateRequest(this.patientForm.value);
                patientUpdateRequest.id = this.patientId;
                this.httpService.put('patient', patientUpdateRequest, true).subscribe({
                    next: (response: any) => {
                        console.log('Patient updated successfully', response);
                        this.router.navigate(['/patients/' + this.patientId]);
                    },
                    error: (error: any) => {
                        console.error('Error updating patient', error);
                    }
                });
            }
            else {
                const patientCreateRequest = new PatientCreateRequest(this.patientForm.value);
                this.httpService.post('patient', patientCreateRequest, true).subscribe({
                    next: (response: any) => {
                        console.log('Patient created successfully', response);
                        this.router.navigate(['/patients/' + response.id]);
                    },
                    error: (error: any) => {
                        console.error('Error creating patient', error);
                    }
                });
            }
        }
    }
}
