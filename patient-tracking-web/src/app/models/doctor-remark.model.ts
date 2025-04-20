export class DoctorRemark {
    id: number;
    patientId: number;
    date: Date;
    description: string;

    constructor(data: any) {
        this.id = data.id;
        this.patientId = data.patientId;
        this.date = data.date;
        this.description = data.description;
    }
}