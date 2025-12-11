import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfigService } from './app-config.service';
import { LoginRequest, LoginResponse } from '../../data/interfaces/auth.Interface';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private config = inject(AppConfigService);

  login(credentials: LoginRequest): Observable<LoginResponse> {
    // Aquí usamos la URL que vino del JSON + el endpoint del controller
    const url = `${this.config.apiUrl}/Auth/login`; 
    return this.http.post<LoginResponse>(url, credentials);
  }
}