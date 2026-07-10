import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../../core/config/api-config';
import { SummaryDto } from '../../providers/models/provider.model';

@Injectable({ providedIn: 'root' })
export class SummaryApiService {
  private readonly http = inject(HttpClient);

  getSummary(): Observable<SummaryDto> {
    return this.http.get<SummaryDto>(`${API_BASE_URL}/summary`);
  }
}
