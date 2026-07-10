using ProviderServices.Application.DTOs;
using ProviderServices.Domain.Entities;

namespace ProviderServices.Application.Interfaces;

public interface IProviderRepository
{
    Task<Provider?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsByNitAsync(string nit, CancellationToken ct = default);
    Task<(IReadOnlyList<Provider> Items, int Total)> GetAllAsync(ProviderFilter filter, CancellationToken ct = default);
    Task AddAsync(Provider provider, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}

public interface IServiceRepository
{
    Task<Service?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<(IReadOnlyList<Service> Items, int Total)> GetByProviderIdAsync(Guid providerId, ServiceFilter filter, CancellationToken ct = default);
    Task AddAsync(Service service, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}