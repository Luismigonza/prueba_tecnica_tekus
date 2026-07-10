using ProviderServices.Application.DTOs;

namespace ProviderServices.Application.Interfaces;

public interface IProvidersAppService
{
    Task<ProviderDto> CreateAsync(CreateProviderRequest request, CancellationToken ct = default);
    Task<ProviderDto> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResult<ProviderDto>> GetAllAsync(ProviderFilter filter, CancellationToken ct = default);
    Task<ProviderDto> UpdateAsync(Guid id, UpdateProviderRequest request, CancellationToken ct = default);
}

public interface IServicesAppService
{
    Task<ServiceDto> CreateAsync(Guid providerId, CreateServiceRequest request, CancellationToken ct = default);
    Task<PagedResult<ServiceDto>> GetByProviderIdAsync(Guid providerId, ServiceFilter filter, CancellationToken ct = default);
}

public interface ISummaryQueries
{
    Task<SummaryDto> GetSummaryAsync(CancellationToken ct = default);
}