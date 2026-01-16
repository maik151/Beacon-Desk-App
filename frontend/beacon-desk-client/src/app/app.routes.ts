import { Routes } from '@angular/router';
import { LoginPageComponent } from './features/auth/pages/login-page/login-page.component';


export const routes: Routes = [
  {
    path: 'auth/login',
    component: LoginPageComponent
  },
  {
    path: 'dashboard',
    
    loadComponent: () => import('./features/dashboard/pages/dashboard-page/dashboard-page')
      .then(m => m.DashboardPageComponent)
  },
  {
    path: '',
    redirectTo: 'auth/login',
    pathMatch: 'full'
  }
];

