import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginRequest } from '@data/interfaces/auth.interface'; // Asegura tus paths o usa rutas relativas

@Component({
  selector: 'app-login-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <form [formGroup]="form" (ngSubmit)="onSubmit()" class="space-y-6">
      <div>
        <label class="block text-sm font-medium text-gray-700">Email</label>
        <input 
          formControlName="email" 
          type="email" 
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 p-2 border"
          placeholder="admin@beacondesk.com">
      </div>

      <div>
        <label class="block text-sm font-medium text-gray-700">Password</label>
        <input 
          formControlName="password" 
          type="password" 
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 p-2 border">
      </div>

      <button 
        type="submit" 
        [disabled]="form.invalid || isLoading"
        class="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none disabled:bg-gray-400">
        {{ isLoading ? 'Cargando...' : 'Iniciar Sesión' }}
      </button>

      <p *ngIf="errorMessage" class="text-red-500 text-sm text-center mt-2">
        {{ errorMessage }}
      </p>
    </form>
  `
})
export class LoginFormComponent {
  @Input() isLoading = false;
  @Input() errorMessage: string | null = null;
  @Output() login = new EventEmitter<LoginRequest>();

  form: FormGroup;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit() {
    if (this.form.valid) {
      this.login.emit(this.form.value);
    }
  }
}