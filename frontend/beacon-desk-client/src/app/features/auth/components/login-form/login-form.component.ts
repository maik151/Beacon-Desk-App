import { Component, EventEmitter, Input, Output, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { InputComponent } from '../../../../shared/components/ui/input/input/input';
import { ButtonComponent } from '../../../../shared/components/ui/button/button';
import { LoginRequest } from '../../../../data/interfaces/auth.interface';

@Component({
  selector: 'app-login-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, InputComponent, ButtonComponent],
  template: `
    <form [formGroup]="form" (ngSubmit)="submit()" class="space-y-6">
      
      <app-input 
        formControlName="email"
        label="Usuario"
        placeholder="usuario@beacon.com"
        [error]="getError('email')"
      ></app-input>

      <app-input 
        formControlName="password"
        label="Contraseña"
        [type]="showPassword() ? 'text' : 'password'"
        [error]="getError('password')"
        [hasSuffix]="true"
      >
        <button type="button" suffix (click)="togglePassword()" class="hover:text-primary transition-colors">
          <span class="material-symbols-outlined text-xl align-middle">
            {{ showPassword() ? 'visibility' : 'visibility_off' }}
          </span>
        </button>
      </app-input>

      <div class="flex justify-end">
        <a routerLink="/auth/recovery" class="text-sm font-medium text-text-secondary hover:text-primary transition-colors">
          ¿Olvidaste tu Contraseña?
        </a>
      </div>

      <app-button 
        type="submit" 
        [loading]="isLoading" 
        [disabled]="form.invalid || isLoading"
      >
        Ingresar
      </app-button>
    </form>
  `
})
export class LoginFormComponent {
  @Input() isLoading = false; // Recibe estado del padre
  @Output() onSubmit = new EventEmitter<LoginRequest>();
  
  private fb = inject(FormBuilder);
  showPassword = signal(false);

  form: FormGroup = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  togglePassword() {
    this.showPassword.update(v => !v);
  }

  getError(field: string): string | null {
    const control = this.form.get(field);
    if (control?.invalid && (control.dirty || control.touched)) {
      if (control.hasError('required')) return 'Este campo es requerido';
      if (control.hasError('email')) return 'Formato de correo inválido';
      if (control.hasError('minlength')) return 'Mínimo 6 caracteres';
    }
    return null;
  }

  submit() {
    if (this.form.valid) {
      this.onSubmit.emit(this.form.getRawValue());
    } else {
      this.form.markAllAsTouched();
    }
  }
}