import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from './features/auth/services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, MatToolbarModule, MatButtonModule],
  template: `
    <mat-toolbar color="primary">
      <span class="brand">Provider Services</span>
      @if (authService.isAuthenticated()) {
        <nav>
          <a mat-button routerLink="/providers" routerLinkActive="active">Providers</a>
          <a mat-button routerLink="/summary" routerLinkActive="active">Summary</a>
          <button mat-button (click)="logout()">Logout</button>
        </nav>
      }
    </mat-toolbar>

    <main class="content">
      <router-outlet />
    </main>
  `,
  styles: [`
    .brand { font-weight: 600; margin-right: 24px; }
    nav { display: flex; gap: 4px; }
    .active { text-decoration: underline; }
    .content { display: block; max-width: 1100px; margin: 0 auto; padding: 24px 16px; }
  `]
})
export class AppComponent {
  protected readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
