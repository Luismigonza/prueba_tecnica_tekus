export interface ServiceDto {
  id: string;
  name: string;
  hourlyRateUsd: number;
  providerId: string;
  createdAt: string;
}

export interface ProviderDto {
  id: string;
  nit: string;
  name: string;
  website: string;
  email: string;
  country: string;
  createdAt: string;
  updatedAt: string | null;
}

export interface PagedFilter {
  search?: string | null;
  sortBy?: string | null;
  sortDescending?: boolean;
  page: number;
  pageSize: number;
}

export interface ProviderFilter extends PagedFilter {
  country?: string | null;
}

export type ServiceFilter = PagedFilter;

export interface PagedResult<T> {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
}

export interface CreateProviderRequest {
  nit: string;
  name: string;
  website: string;
  email: string;
  country: string;
}

export interface UpdateProviderRequest {
  name: string;
  website: string;
  email: string;
  country: string;
}

export interface CreateServiceRequest {
  name: string;
  hourlyRateUsd: number;
}

export interface CountByCountry {
  country: string;
  count: number;
}

export interface SummaryDto {
  providersByCountry: CountByCountry[];
  servicesByCountry: CountByCountry[];
}
