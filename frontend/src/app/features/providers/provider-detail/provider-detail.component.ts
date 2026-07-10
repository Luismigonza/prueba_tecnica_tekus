import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDividerModule } from '@angular/material/divider';

import { ProviderApiService } from '../services/provider-api.service';
import { ProviderDto, ServiceDto } from '../models/provider.model';

@Component({
  selector: 'app-provider-detail',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatSnackBarModule,
    MatDividerModule
  ],
  template: `
    @if (provider(); as p) {
      <div class="cabecera">
        <h1>{{ p.name }}</h1>
        <a mat-stroked-button [routerLink]="['/providers', p.id, 'edit']">Edit</a>
      </div>

      <p class="meta">Nit: {{ p.nit }} — Country: {{ p.country }}</p>
      <p class="meta">Website: {{ p.website }} — Email: {{ p.email }}</p>

      <mat-divider></mat-divider>

      <section>
        <h3>Services ({{ services().length }})</h3>

        <table mat-table [dataSource]="services()" class="tabla">
          <ng-container matColumnDef="name">
            <th mat-header-cell *matHeaderCellDef>Name</th>
            <td mat-cell *matCellDef="let s">{{ s.name }}</td>
          </ng-container>

          <ng-container matColumnDef="hourlyRateUsd">
            <th mat-header-cell *matHeaderCellDef>Hourly rate (USD)</th>
            <td mat-cell *matCellDef="let s">{{ s.hourlyRateUsd | currency }}</td>
          </ng-container>

          <tr mat-header-row *matHeaderRowDef="columnas"></tr>
          <tr mat-row *matRowDef="let row; columns: columnas"></tr>
        </table>

        @if (services().length === 0) {
          <p class="vacio">This provider has no services yet.</p>
        }

        <form [formGroup]="form" (ngSubmit)="addService(p.id)" class="form-service">
          <mat-form-field appearance="outline">
            <mat-label>Service name</mat-label>
            <input matInput formControlName="name" maxlength="200" />
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Hourly rate (USD)</mat-label>
            <input matInput type="number" step="0.01" formControlName="hourlyRateUsd" />
          </mat-form-field>

          <button mat-raised-button color="primary" type="submit" [disabled]="form.invalid || saving()">
            Add service
          </button>
        </form>
      </section>
    }
  `,
  styles: [`
    .cabecera { display: flex; justify-content: space-between; align-items: center; }
    .meta { color: #555; font-size: 13px; margin: 2px 0; }
    .tabla { width: 100%; margin-top: 12px; }
    .vacio { color: #666; padding: 8px 0; }
    .form-service { display: flex; gap: 12px; align-items: flex-start; margin-top: 16px; }
  `]
})
export class ProviderDetailComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly providerApi = inject(ProviderApiService);
  private readonly snackBar = inject(MatSnackBar);

  readonly columnas = ['name', 'hourlyRateUsd'];

  provider = signal<ProviderDto | null>(null);
  services = signal<ServiceDto[]>([]);
  saving = signal(false);

  form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(200)]],
    hourlyRateUsd: [0, [Validators.required, Validators.min(0.01)]]
  });

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.providerApi.getById(id).subscribe((provider) => this.provider.set(provider));
    this.loadServices(id);
  }

  addService(providerId: string): void {
    if (this.form.invalid) return;
    this.saving.set(true);
    const values = this.form.getRawValue();

    this.providerApi.addService(providerId, { name: values.name!, hourlyRateUsd: values.hourlyRateUsd! }).subscribe({
      next: () => {
        this.form.reset({ name: '', hourlyRateUsd: 0 });
        this.saving.set(false);
        this.snackBar.open('Service added. Notification email dispatched.', 'Close', { duration: 3000 });
        this.loadServices(providerId);
      },
      error: (err) => {
        this.saving.set(false);
        this.snackBar.open(err.friendlyMessage ?? 'Could not add the service.', 'Close', { duration: 4000 });
      }
    });
  }

  private loadServices(providerId: string): void {
    this.providerApi.listServices(providerId, { page: 1, pageSize: 50 }).subscribe((result) => {
      this.services.set(result.items);
    });
  }
}
