export class PredictionResult {
    id: number;
    patientId: number;
    detail: string;

    constructor(data: any) {
        this.id = data.id;
        this.patientId = data.patientId;
        this.detail = data.detail;
    }
}