import { ApplicationConfig, APP_INITIALIZER, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { provideHttpClient, withFetch } from '@angular/common/http';

import { routes } from './app.routes';
import { AppConfigService } from './core/services/app-config.service';

import { withInterceptors } from '@angular/common/http';
import { authInterceptor } from './core/interceptors/auth.interceptors';


// Función para cargar el JSON antes de iniciar
export function initConfig(configService: AppConfigService) {
  return () => configService.loadConfig();
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    
    // 1. SOLUCIÓN ERROR DE RUTAS
    provideRouter(routes, withComponentInputBinding()), 
    
    // 2. SOLUCIÓN ERROR HTTP CLIENT (Esto es lo que te falta)
    provideHttpClient(
      withFetch(),
      withInterceptors([authInterceptor])
    ), 

    // 3. Inicialización de tu Config.json
    {
      provide: APP_INITIALIZER,
      useFactory: initConfig,
      deps: [AppConfigService],
      multi: true
    }
  ]
};