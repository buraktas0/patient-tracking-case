import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { TokenService } from './token.service';

@Injectable({
    providedIn: 'root'
})
export class HttpService {

    private apiUrl = environment.apiUrl;

    constructor(
        private http: HttpClient,
        private tokenService: TokenService
    ) { }

    get<T>(endpoint: string, isAuthRequired: boolean = false) {
        const headers = this.getHeaders(isAuthRequired);
        return this.http.get<T>(`${this.apiUrl}${endpoint}`, { headers });
    }

    post<T>(endpoint: string, data: any, isAuthRequired: boolean = false) {
        const headers = this.getHeaders(isAuthRequired);
        return this.http.post<T>(`${this.apiUrl}${endpoint}`, data, { headers });
    }

    put<T>(endpoint: string, data: any, isAuthRequired: boolean = false) {
        const headers = this.getHeaders(isAuthRequired);
        return this.http.put<T>(`${this.apiUrl}${endpoint}`, data, { headers });
    }

    delete<T>(endpoint: string, isAuthRequired: boolean = false) {
        const headers = this.getHeaders(isAuthRequired);
        return this.http.delete<T>(`${this.apiUrl}${endpoint}`, { headers });
    }

    private getHeaders(authRequired: boolean) {
        if (authRequired) {
            const token = this.tokenService.getToken();
            return new HttpHeaders({
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            });
        }
        else {
            return new HttpHeaders({
                'Content-Type': 'application/json'
            });
        }
    }
}