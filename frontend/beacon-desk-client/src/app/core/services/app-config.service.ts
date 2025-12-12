import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AppConfigService {
  private http = inject(HttpClient);
  private config: any = {};

  get apiUrl(): string {
    return this.config.apiUrl;
  }

  async loadConfig(): Promise<void> {
    try {
      // Leemos el archivo físico de assets
      const data = await lastValueFrom(this.http.get('/assets/config.json'));
      this.config = data;
    } catch (error) {
      console.error('Error cargando config.json', error);
    }
  }
}