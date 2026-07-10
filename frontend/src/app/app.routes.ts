import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'providers', pathMatch: 'full' },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login.component').then((m) => m.LoginComponent)
  },
  {
    path: 'providers',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/providers/provider-list/provider-list.component').then((m) => m.ProviderListComponent)
  },
  {
    path: 'providers/new',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/providers/provider-form/provider-form.component').then((m) => m.ProviderFormComponent)
  },
  {
    path: 'providers/:id/edit',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/providers/provider-form/provider-form.component').then((m) => m.ProviderFormComponent)
  },
  {
    path: 'providers/:id',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/providers/provider-detail/provider-detail.component').then((m) => m.ProviderDetailComponent)
  },
  {
    path: 'summary',
    canActivate: [authGuard],
    loadComponent: () => import('./features/summary/summary.component').then((m) => m.SummaryComponent)
  },
  { path: '**', redirectTo: 'providers' }
];
