import { Injectable, signal, effect, inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private platformId = inject(PLATFORM_ID);

  // Signal principal: true = Dark Mode, false = Light Mode
  isDarkMode = signal<boolean>(false);

  constructor() {
    // Solo ejecutamos lógica del navegador si estamos en el cliente (evita errores de SSR)
    if (isPlatformBrowser(this.platformId)) {
      this.initializeTheme();
    }

    // Effect: Reacciona automáticamente cuando cambia el signal isDarkMode
    effect(() => {
      if (isPlatformBrowser(this.platformId)) {
        if (this.isDarkMode()) {
          document.documentElement.classList.add('dark-theme');
          document.documentElement.classList.add('dark'); // Compatibilidad con Tailwind
          localStorage.setItem('theme', 'dark');
        } else {
          document.documentElement.classList.remove('dark-theme');
          document.documentElement.classList.remove('dark');
          localStorage.setItem('theme', 'light');
        }
      }
    });
  }

  toggleTheme() {
    this.isDarkMode.update((current) => !current);
  }

  private initializeTheme() {
    // 1. Revisar localStorage
    const savedTheme = localStorage.getItem('theme');
    
    if (savedTheme) {
      this.isDarkMode.set(savedTheme === 'dark');
    } else {
      // 2. Si no hay nada guardado, revisar preferencia del sistema operativo
      const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
      this.isDarkMode.set(prefersDark);
    }
  }
}