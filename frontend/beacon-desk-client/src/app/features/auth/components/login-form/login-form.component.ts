import { Component, EventEmitter, Input, Output, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { InputComponent } from '../../../../shared/components/ui/input/input/input';
import { ButtonComponent } from '../../../../shared/components/ui/button/button';
import { LoginRequest } from '../../../../data/interfaces/auth.interface';
import { RouterLink } from '@angular/router'; // Importante para el link

@Component({
  selector: 'app-login-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, InputComponent, ButtonComponent, RouterLink],
  template: `
    <form [formGroup]="form" (ngSubmit)="submit()" class="w-full">
      
      <div class="mb-6">
        <app-input 
          formControlName="email"
          label="Usuario"
          placeholder="usuario@beacon.com"
          [error]="getError('email')"
        ></app-input>
      </div>

      <div class="mb-10">
        
        <div class="flex justify-between items-center mb-2">
          <label class="text-sm font-bold text-text-main">Contraseña</label>
          
          <a routerLink="/auth/recovery" class="text-xs font-medium text-text-secondary hover:text-primary transition-colors cursor-pointer">
            ¿Olvidaste tu Contraseña?
          </a>
        </div>

        <app-input 
          formControlName="password"
          placeholder="............"
          [type]="showPassword() ? 'text' : 'password'"
          [error]="getError('password')"
          [hasSuffix]="true"
        >
          <button type="button" suffix (click)="togglePassword()" class="text-text-secondary hover:text-primary transition-colors flex items-center justify-center h-full px-2">
            <span class="material-symbols-outlined text-xl">
              {{ showPassword() ? 'visibility' : 'visibility_off' }}
            </span>
          </button>
        </app-input>

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
  @Input() isLoading = false;
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
      if (control.hasError('required')) return 'Requerido';
      if (control.hasError('email')) return 'Correo inválido';
      if (control.hasError('minlength')) return 'Mín. 6 caracteres';
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