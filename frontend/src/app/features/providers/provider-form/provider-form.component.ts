import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { ProviderApiService } from '../services/provider-api.service';

@Component({
  selector: 'app-provider-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, MatFormFieldModule, MatInputModule, MatButtonModule, MatSnackBarModule],
  template: `
    <h1>{{ isEditMode() ? 'Edit provider' : 'New provider' }}</h1>

    <form [formGroup]="form" (ngSubmit)="save()" class="form">
      @if (!isEditMode()) {
        <mat-form-field appearance="outline">
          <mat-label>Nit</mat-label>
          <input matInput formControlName="nit" maxlength="20" />
        </mat-form-field>
      }

      <mat-form-field appearance="outline">
        <mat-label>Name</mat-label>
        <input matInput formControlName="name" maxlength="200" />
      </mat-form-field>

      <mat-form-field appearance="outline">
        <mat-label>Website</mat-label>
        <input matInput formControlName="website" placeholder="https://..." />
      </mat-form-field>

      <mat-form-field appearance="outline">
        <mat-label>Email</mat-label>
        <input matInput type="email" formControlName="email" />
      </mat-form-field>

      <mat-form-field appearance="outline">
        <mat-label>Country</mat-label>
        <input matInput formControlName="country" maxlength="100" />
      </mat-form-field>

      <div class="acciones">
        <a mat-button routerLink="/providers">Cancel</a>
        <button mat-raised-button color="primary" type="submit" [disabled]="form.invalid || saving()">
          {{ saving() ? 'Saving...' : 'Save' }}
        </button>
      </div>
    </form>
  `,
  styles: [`
    .form { display: flex; flex-direction: column; gap: 4px; max-width: 480px; }
    .acciones { display: flex; justify-content: flex-end; gap: 12px; margin-top: 8px; }
  `]
})
export class ProviderFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly providerApi = inject(ProviderApiService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  providerId = signal<string | null>(null);
  isEditMode = signal(false);
  saving = signal(false);

  form = this.fb.group({
    nit: ['', [Validators.required, Validators.maxLength(20)]],
    name: ['', [Validators.required, Validators.maxLength(200)]],
    website: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    country: ['', [Validators.required, Validators.maxLength(100)]]
  });

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.providerId.set(id);
      this.isEditMode.set(true);
      this.form.get('nit')?.clearValidators();

      this.providerApi.getById(id).subscribe((provider) => {
        this.form.patchValue({
          name: provider.name,
          website: provider.website,
          email: provider.email,
          country: provider.country
        });
      });
    }
  }

  save(): void {
    if (this.form.invalid) return;
    this.saving.set(true);
    const values = this.form.getRawValue();

    const operation = this.isEditMode()
      ? this.providerApi.update(this.providerId()!, {
          name: values.name!,
          website: values.website!,
          email: values.email!,
          country: values.country!
        })
      : this.providerApi.create({
          nit: values.nit!,
          name: values.name!,
          website: values.website!,
          email: values.email!,
          country: values.country!
        });

    operation.subscribe({
      next: (provider) => {
        this.snackBar.open('Provider saved.', 'Close', { duration: 3000 });
        this.router.navigate(['/providers', provider.id]);
      },
      error: (err) => {
        this.saving.set(false);
        this.snackBar.open(err.friendlyMessage ?? 'Could not save the provider.', 'Close', { duration: 4000 });
      }
    });
  }
}
