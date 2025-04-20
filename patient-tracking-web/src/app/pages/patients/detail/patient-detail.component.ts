import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Patient } from '../../../models/patient.model';
import { HttpService } from '../../../services/http.service';
import { HistoricalRecord } from '../../../models/historical-record.model';
import { DoctorRemark } from '../../../models/doctor-remark.model';
import { PredictionResult } from '../../../models/prediction-result.model';

@Component({
    selector: 'app-patient-detail',
    templateUrl: './patient-detail.component.html',
    imports: [CommonModule]
})
export class PatientDetailComponent {
    patientId?: number;
    patient?: Patient;
    historicalRecords: HistoricalRecord[] = [];
    doctorRemarks: DoctorRemark[] = [];
    predictions: PredictionResult[] = [];

    showRecords: boolean = false;
    showRemarks: boolean = false;
    isLoading: boolean = false;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private httpService: HttpService
    ) { }

    ngOnInit() {
        this.patientId = this.route.snapshot.params['id'];
        if (this.patientId) {
            this.fetchPatient(this.patientId);
            this.fetchHistoricalRecords(this.patientId);
            this.fetchDoctorRemarks(this.patientId);
        }
        else {
            this.router.navigate(['/patients']);
        }
    }

    fetchPatient(patientId: number) {
        this.httpService.get<Patient>(`patient/${patientId}`, true).subscribe({
            next: (response: Patient) => {
                console.log('Patient fetched successfully', response);
                this.patient = response;
            },
            error: (error: any) => {
                console.error('Error fetching patient', error);
            }
        });
    }

    fetchHistoricalRecords(patientId: number) {
        this.historicalRecords = [
            { id: 1, patientId: patientId, date: new Date(2025, 0, 1), description: 'Record 1 - Lorem ipsum dolor sit amet' },
            { id: 2, patientId: patientId, date: new Date(2025, 1, 1), description: 'Record 2 - consectetur adipiscing elit' },
            { id: 3, patientId: patientId, date: new Date(2025, 2, 1), description: 'Record 3 - sed do eiusmod tempor incididunt' },
            { id: 4, patientId: patientId, date: new Date(2025, 3, 1), description: 'Record 4 - ut labore et dolore magna aliqua.' },
            { id: 5, patientId: patientId, date: new Date(2025, 4, 1), description: 'Record 5 - Ut enim ad minim veniam' },
            { id: 6, patientId: patientId, date: new Date(2025, 5, 1), description: 'Record 6 - quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat' },
            { id: 7, patientId: patientId, date: new Date(2025, 6, 1), description: 'Record 7 - Duis aute irure dolor in reprehenderit' },
            { id: 8, patientId: patientId, date: new Date(2025, 7, 1), description: 'Record 8 - in voluptate velit esse cillum dolore' },
            { id: 9, patientId: patientId, date: new Date(2025, 8, 1), description: 'Record 9 - eu fugiat nulla pariatur' },
            { id: 10, patientId: patientId, date: new Date(2025, 9, 1), description: 'Record 10 - Excepteur sint occaecat cupidatat non proident' },
            { id: 11, patientId: patientId, date: new Date(2025, 10, 1), description: 'Record 11 - sunt in culpa qui officia deserunt' },
            { id: 12, patientId: patientId, date: new Date(2025, 11, 1), description: 'Record 12 - mollit anim id est laborum' },
        ];        
    }

    fetchDoctorRemarks(patientId: number) {
        this.doctorRemarks = [
            { id: 1, patientId: patientId, date: new Date(2025, 0, 1), description: 'Remark 1 - Lorem ipsum dolor sit amet' },
            { id: 2, patientId: patientId, date: new Date(2025, 1, 1), description: 'Remark 2 - consectetur adipiscing elit' },
            { id: 3, patientId: patientId, date: new Date(2025, 2, 1), description: 'Remark 3 - sed do eiusmod tempor incididunt' }
        ];
    }

    fetchPredictions() {
        console.log('fetchPredictions');
    }

    toggleRecords() {
        this.showRecords = !this.showRecords;
    }

    toggleRemarks() {
        this.showRemarks = !this.showRemarks;
    }

    onPredict() {
        this.isLoading = true;
        setTimeout(() => {
            this.httpService.get<PredictionResult[]>(`prediction/${this.patientId}`, true).subscribe({
                next: (response: PredictionResult[]) => {
                    console.log('Predictions fetched successfully', response);
                    this.predictions = response;
                },
                error: (error: any) => {
                    console.error('Error fetching predictions', error);
                }
            });
            
            this.isLoading = false;
        }
        , 3000);
    }

    goBack() {
        this.router.navigate(['/patients']);
    }
}