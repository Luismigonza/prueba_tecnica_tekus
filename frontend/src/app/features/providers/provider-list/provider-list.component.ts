import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { ProviderApiService } from '../services/provider-api.service';
import { ProviderDto, ProviderFilter } from '../models/provider.model';

@Component({
  selector: 'app-provider-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    MatTableModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatPaginatorModule,
    MatProgressSpinnerModule
  ],
  template: `
    <div class="cabecera">
      <h1>Providers</h1>
      <a mat-raised-button color="primary" routerLink="/providers/new">
        <mat-icon>add</mat-icon> New provider
      </a>
    </div>

    <mat-form-field appearance="outline" class="buscador">
      <mat-label>Search</mat-label>
      <input matInput [(ngModel)]="search" (keyup.enter)="aplicarFiltros()" placeholder="Name, Nit or email" />
    </mat-form-field>
    <button mat-stroked-button (click)="aplicarFiltros()"><mat-icon>search</mat-icon> Search</button>

    @if (cargando()) {
      <mat-spinner diameter="36"></mat-spinner>
    } @else {
      <table mat-table [dataSource]="items()" matSort (matSortChange)="ordenar($event)" class="tabla">
        <ng-container matColumnDef="name">
          <th mat-header-cell *matHeaderCellDef mat-sort-header>Name</th>
          <td mat-cell *matCellDef="let p"><a [routerLink]="['/providers', p.id]">{{ p.name }}</a></td>
        </ng-container>

        <ng-container matColumnDef="nit">
          <th mat-header-cell *matHeaderCellDef mat-sort-header>Nit</th>
          <td mat-cell *matCellDef="let p">{{ p.nit }}</td>
        </ng-container>

        <ng-container matColumnDef="country">
          <th mat-header-cell *matHeaderCellDef mat-sort-header>Country</th>
          <td mat-cell *matCellDef="let p">{{ p.country }}</td>
        </ng-container>

        <ng-container matColumnDef="email">
          <th mat-header-cell *matHeaderCellDef>Email</th>
          <td mat-cell *matCellDef="let p">{{ p.email }}</td>
        </ng-container>

        <tr mat-header-row *matHeaderRowDef="columnas"></tr>
        <tr mat-row *matRowDef="let row; columns: columnas"></tr>
      </table>

      @if (items().length === 0) {
        <p class="vacio">No providers match this filter.</p>
      }

      <mat-paginator
        [length]="total()"
        [pageSize]="filtro().pageSize"
        [pageIndex]="filtro().page - 1"
        [pageSizeOptions]="[5, 10, 25]"
        (page)="cambiarPagina($event)">
      </mat-paginator>
    }
  `,
  styles: [`
    .cabecera { display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px; }
    .buscador { margin-right: 8px; width: 280px; }
    .tabla { width: 100%; margin-top: 12px; }
    .vacio { color: #666; padding: 16px 0; }
  `]
})
export class ProviderListComponent {
  private readonly providerApi = inject(ProviderApiService);

  readonly columnas = ['name', 'nit', 'country', 'email'];

  search = '';
  filtro = signal<ProviderFilter>({ page: 1, pageSize: 10 });
  items = signal<ProviderDto[]>([]);
  total = signal(0);
  cargando = signal(true);

  constructor() {
    this.cargar();
  }

  aplicarFiltros(): void {
    this.filtro.set({ ...this.filtro(), search: this.search || undefined, page: 1 });
    this.cargar();
  }

  ordenar(sort: Sort): void {
    this.filtro.set({
      ...this.filtro(),
      sortBy: sort.direction ? sort.active : undefined,
      sortDescending: sort.direction === 'desc'
    });
    this.cargar();
  }

  cambiarPagina(evento: PageEvent): void {
    this.filtro.set({ ...this.filtro(), page: evento.pageIndex + 1, pageSize: evento.pageSize });
    this.cargar();
  }

  private cargar(): void {
    this.cargando.set(true);
    this.providerApi.list(this.filtro()).subscribe({
      next: (resultado) => {
        this.items.set(resultado.items);
        this.total.set(resultado.total);
        this.cargando.set(false);
      },
      error: () => this.cargando.set(false)
    });
  }
}
