import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../../core/config/api-config';
import {
  CreateProviderRequest,
  CreateServiceRequest,
  PagedFilter,
  PagedResult,
  ProviderDto,
  ProviderFilter,
  ServiceDto,
  ServiceFilter,
  UpdateProviderRequest
} from '../models/provider.model';

function buildParams(filter: PagedFilter): HttpParams {
  let params = new HttpParams().set('page', filter.page).set('pageSize', filter.pageSize);
  if (filter.search) params = params.set('search', filter.search);
  if (filter.sortBy) params = params.set('sortBy', filter.sortBy);
  if (filter.sortDescending) params = params.set('sortDescending', filter.sortDescending);
  return params;
}

@Injectable({ providedIn: 'root' })
export class ProviderApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${API_BASE_URL}/providers`;

  list(filter: ProviderFilter): Observable<PagedResult<ProviderDto>> {
    let params = buildParams(filter);
    if (filter.country) params = params.set('country', filter.country);
    return this.http.get<PagedResult<ProviderDto>>(this.baseUrl, { params });
  }

  getById(id: string): Observable<ProviderDto> {
    return this.http.get<ProviderDto>(`${this.baseUrl}/${id}`);
  }

  create(request: CreateProviderRequest): Observable<ProviderDto> {
    return this.http.post<ProviderDto>(this.baseUrl, request);
  }

  update(id: string, request: UpdateProviderRequest): Observable<ProviderDto> {
    return this.http.put<ProviderDto>(`${this.baseUrl}/${id}`, request);
  }

  listServices(providerId: string, filter: ServiceFilter): Observable<PagedResult<ServiceDto>> {
    const params = buildParams(filter);
    return this.http.get<PagedResult<ServiceDto>>(`${this.baseUrl}/${providerId}/services`, { params });
  }

  addService(providerId: string, request: CreateServiceRequest): Observable<ServiceDto> {
    return this.http.post<ServiceDto>(`${this.baseUrl}/${providerId}/services`, request);
  }
}
