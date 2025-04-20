import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Patient } from '../../models/patient.model';
import { Router } from '@angular/router';
import { HttpService } from '../../services/http.service';
@Component({
    selector: 'app-patients',
    templateUrl: './patients.component.html',
    imports: [CommonModule]
})
export class PatientsComponent {
    patients: Patient[] = [];
    showDeleteConfirmation: boolean = false;
    selectedPatient: Patient | null = null;

    constructor(
        private router: Router,
        private httpService: HttpService
    ) { }

    ngOnInit() {
        this.fetchPatients();
    }

    fetchPatients() {
        this.httpService.get<Patient[]>('patient', true).subscribe({
            next: (response: Patient[]) => {
                console.log('Patients fetched successfully', response);
                this.patients = response;
            },
            error: (error: any) => {
                console.error('Error fetching patients', error);
            }
        });
    }

    onAddPatient() {
        this.router.navigate(['/patients/create']);
    }

    onPatientClick(patient: Patient) {
        console.log(patient);
        this.router.navigate(['/patients', patient.id]);
    }

    onEditPatient(patient: Patient) {
        this.router.navigate(['/patients/edit', patient.id]);
    }

    onDeletePatient(patient: Patient) {
        this.selectedPatient = patient;
        this.showDeleteConfirmation = true;
    }

    cancelDelete() {
        this.showDeleteConfirmation = false;
        this.selectedPatient = null;
    }

    confirmDelete() {
        if (this.selectedPatient) {
            this.httpService.delete(`patient/${this.selectedPatient.id}`, true).subscribe({
                next: () => {
                    console.log('Patient deleted successfully');
                    this.showDeleteConfirmation = false;
                    this.selectedPatient = null;
                    this.fetchPatients();
                },
                error: (error: any) => {
                    console.error('Error deleting patient', error);
                }
            });
        }
    }
}
