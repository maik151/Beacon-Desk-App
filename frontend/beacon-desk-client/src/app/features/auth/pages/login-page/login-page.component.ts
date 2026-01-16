import { Component, inject, signal } from '@angular/core';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthService } from '../../../../core/auth/services/auth.service';
import { ThemeService } from '../../../../core/services/theme.service';
import { LoginFormComponent } from '../../components/login-form/login-form.component';
import { LoginRequest } from '../../../../data/interfaces/auth.interface';
import { ProblemDetails } from '../../../../data/interfaces/api-response.interface';
import {LogoComponent} from '../../../../shared/components/ui/logo/logo/logo';

@Component({
  selector: 'app-login-page',
  standalone: true,
  imports: [CommonModule, LoginFormComponent, LogoComponent],
  templateUrl: './login-page.component.html'
})
export class LoginPageComponent {
  private authService = inject(AuthService);
  private router = inject(Router);
  public themeService = inject(ThemeService);

  isLoading = signal(false);
  errorMessage = signal<string | null>(null);

  handleLogin(credentials: LoginRequest) {
    this.isLoading.set(true);
    this.errorMessage.set(null); // Limpiar errores previos

    this.authService.login(credentials).subscribe({
      next: (response) => {
        // El servicio ya guarda el token y el usuario en el signal
        this.isLoading.set(false);
        // Toast de éxito opcional: console.log(response.message);
        this.router.navigate(['/dashboard']);
      },
      error: (err: HttpErrorResponse) => {
        this.isLoading.set(false);
        
        // Manejo del Error Estándar (RFC 7807)
        if (err.error) {
          const problem = err.error as ProblemDetails;
          // Mostramos 'detail' ("Credenciales inválidas") o un fallback
          this.errorMessage.set(problem.detail || 'Ocurrió un error inesperado');
        } else {
          this.errorMessage.set('Error de conexión con el servidor');
        }
      }
    });
  }
}