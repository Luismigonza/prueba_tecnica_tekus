import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { SummaryApiService } from './services/summary-api.service';
import { SummaryDto } from '../providers/models/provider.model';

@Component({
  selector: 'app-summary',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatProgressSpinnerModule],
  template: `
    <h1>Summary</h1>

    @if (loading()) {
      <mat-spinner diameter="36"></mat-spinner>
    } @else if (summary(); as s) {
      <div class="columnas">
        <section>
          <h3>Providers by country</h3>
          @for (item of s.providersByCountry; track item.country) {
            <mat-card class="fila">
              <span>{{ item.country }}</span>
              <strong>{{ item.count }}</strong>
            </mat-card>
          }
        </section>

        <section>
          <h3>Services by country</h3>
          @for (item of s.servicesByCountry; track item.country) {
            <mat-card class="fila">
              <span>{{ item.country }}</span>
              <strong>{{ item.count }}</strong>
            </mat-card>
          }
        </section>
      </div>
    }
  `,
  styles: [`
    .columnas { display: grid; grid-template-columns: 1fr 1fr; gap: 24px; margin-top: 16px; }
    .fila { display: flex; justify-content: space-between; padding: 8px 16px; margin-bottom: 8px; }
  `]
})
export class SummaryComponent implements OnInit {
  private readonly summaryApi = inject(SummaryApiService);

  summary = signal<SummaryDto | null>(null);
  loading = signal(true);

  ngOnInit(): void {
    this.summaryApi.getSummary().subscribe((data) => {
      this.summary.set(data);
      this.loading.set(false);
    });
  }
}
