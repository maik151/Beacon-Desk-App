import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { LoginFormComponent } from '../../components/login-form/login-form.component';
import { AuthService } from '@core/services/auth.service';
import { LoginRequest } from '@data/interfaces/auth.interface';

@Component({
  selector: 'app-login-page',
  standalone: true,
  imports: [CommonModule, LoginFormComponent],
  template: `
    <div class="min-h-screen bg-gray-100 flex flex-col justify-center py-12 sm:px-6 lg:px-8">
      <div class="sm:mx-auto sm:w-full sm:max-w-md">
        <h2 class="mt-6 text-center text-3xl font-extrabold text-gray-900">
          BeaconDesk
        </h2>
        <p class="mt-2 text-center text-sm text-gray-600">
          Accede a tu cuenta
        </p>
      </div>

      <div class="mt-8 sm:mx-auto sm:w-full sm:max-w-md">
        <div class="bg-white py-8 px-4 shadow sm:rounded-lg sm:px-10">
          
          <app-login-form 
            [isLoading]="isLoading"
            [errorMessage]="error"
            (login)="handleLogin($event)">
          </app-login-form>

        </div>
      </div>
    </div>
  `
})
export class LoginPageComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  isLoading = false;
  error: string | null = null;

  handleLogin(credentials: LoginRequest) {
    this.isLoading = true;
    this.error = null;

    this.authService.login(credentials).subscribe({
      next: (response) => {
        console.log('Login Exitoso:', response);
        localStorage.setItem('token', response.token); // Guardado básico temporal
        this.isLoading = false;
        // this.router.navigate(['/dashboard']); // Descomentar cuando exista
      },
      error: (err) => {
        console.error(err);
        this.error = 'Credenciales inválidas o error de servidor';
        this.isLoading = false;
      }
    });
  }
}