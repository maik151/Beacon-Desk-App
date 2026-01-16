import { Component, Input, forwardRef, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-input',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputComponent),
      multi: true
    }
  ],
  template: `
    <div class="flex flex-col gap-1.5 w-full">
      @if (label) {
        <label [for]="inputId" class="text-sm font-medium text-text-main">{{ label }}</label>
      }
      
      <div class="relative group">
        <input
          [id]="inputId"
          [type]="type"
          [placeholder]="placeholder"
          [value]="value()"
          [disabled]="isDisabled()"
          (input)="onInput($event)"
          (blur)="onTouch()"
          class="w-full h-11 px-4 bg-surface-input border border-border rounded-lg text-text-main placeholder:text-text-secondary outline-none transition-all duration-200 focus:border-primary focus:ring-2 focus:ring-primary/20 disabled:opacity-50 disabled:cursor-not-allowed"
          [class.border-error]="!!error"
          [class.focus:border-error]="!!error"
          [class.focus:ring-error]="!!error"
          [class.pr-10]="hasSuffix" 
        />
        
        <div class="absolute right-3 top-1/2 -translate-y-1/2 text-text-secondary">
          <ng-content select="[suffix]"></ng-content>
        </div>
      </div>

      @if (error) {
        <span class="text-xs text-error font-medium animate-slideDown">{{ error }}</span>
      }
    </div>
  `,
  styles: [`
    .animate-slideDown { animation: slideDown 0.2s ease-out; }
    @keyframes slideDown { from { opacity:0; transform:translateY(-2px); } to { opacity:1; transform:translateY(0); } }
  `]
})
export class InputComponent implements ControlValueAccessor {
  @Input() label = '';
  @Input() type = 'text';
  @Input() placeholder = '';
  @Input() inputId = crypto.randomUUID();
  @Input() error: string | null = null;
  @Input() hasSuffix = false;

  value = signal('');
  isDisabled = signal(false);

  onChange = (_: any) => {};
  onTouch = () => {};

  onInput(event: Event) {
    const val = (event.target as HTMLInputElement).value;
    this.value.set(val);
    this.onChange(val);
  }

  writeValue(val: any): void { this.value.set(val || ''); }
  registerOnChange(fn: any): void { this.onChange = fn; }
  registerOnTouched(fn: any): void { this.onTouch = fn; }
  setDisabledState(isDisabled: boolean): void { this.isDisabled.set(isDisabled); }
}