import {Injectable, inject, signal} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import {tap} from 'rxjs/operators';
import {jwtDecode} from 'jwt-decode';
import { AppConfigService } from '../../services/app-config.service';

import {LoginRequest, LoginResponseData, UserTokenPayload, AuthUser} from '../../../data/interfaces/auth.interface';
import { ApiResponse } from '../../../data/interfaces/api-response.interface';

@Injectable({
    providedIn: 'root'
})

export class AuthService {
    private http =  inject(HttpClient);
    private router =    inject(Router);
    private config = inject(AppConfigService);

    currentUser = signal<AuthUser | null>(null);
    isAuthenticated = signal<boolean>(false);

    login(credentials: LoginRequest) {
        const url = `${this.config.apiUrl}/Auth/login`;
        return this.http.post<ApiResponse<LoginResponseData>>(url, credentials)
            .pipe(
                tap(response => {
                    if(response.success && response.data.token){
                        this.handleSuccess(response.data.token);
                    }
                })
            );
    }

    logout(){
        localStorage.removeItem('auth_token');
        this.currentUser.set(null);
        this.isAuthenticated.set(false);
        this.router.navigate(['/auth/login']);
    }

    private handleSuccess(token: string){
        localStorage.setItem('auth_token', token);

        try{
            const decoded = jwtDecode<UserTokenPayload>(token);

            const user: AuthUser = {
                id: decoded.nameId,
                email: decoded.unique_name,
                role: decoded.role 
            };
            this.currentUser.set(user);
            this.isAuthenticated.set(true);

        }catch(error){
            console.error('Error decoding token', error);
        }

    }


    
}
