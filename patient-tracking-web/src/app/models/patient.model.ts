export class Patient  {
    id: number;
    name: string;
    surname: string;
    birthDate: Date;

    constructor(data: any) {
        this.id = data.id;
        this.name = data.name;
        this.surname = data.surname;
        this.birthDate = data.birthDate;
    }
}

export class PatientCreateRequest {
    name: string;
    surname: string;
    birthDate: Date;

    constructor(data: any) {
        this.name = data.name;
        this.surname = data.surname;
        this.birthDate = data.birthDate;
    }
}

export class PatientUpdateRequest { 
    id: number;
    name: string;
    surname: string;
    birthDate: Date;

    constructor(data: any) {
        this.id = data.id;
        this.name = data.name;
        this.surname = data.surname;
        this.birthDate = data.birthDate;
    }
}

export class PatientDeleteRequest {
    id: number;

    constructor(data: any) {
        this.id = data.id;
    }
}