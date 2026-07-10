using ProviderServices.Application.DTOs;
using ProviderServices.Application.Exceptions;
using ProviderServices.Application.Interfaces;
using ProviderServices.Application.Mapping;
using ProviderServices.Domain.Entities;
using ProviderServices.Domain.Exceptions;

namespace ProviderServices.Application.Services;

public class ProvidersAppService : IProvidersAppService
{
    private readonly IProviderRepository _repository;

    public ProvidersAppService(IProviderRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProviderDto> CreateAsync(CreateProviderRequest request, CancellationToken ct = default)
    {
        var nitAlreadyExists = await _repository.ExistsByNitAsync(request.Nit, ct);
        if (nitAlreadyExists)
            throw new DomainException($"A provider with Nit '{request.Nit}' already exists.");

        var provider = Provider.Create(request.Nit, request.Name, request.Website, request.Email, request.Country);

        await _repository.AddAsync(provider, ct);
        await _repository.SaveChangesAsync(ct);

        return provider.ToDto();
    }

    public async Task<ProviderDto> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var provider = await GetOrFailAsync(id, ct);
        return provider.ToDto();
    }

    public async Task<PagedResult<ProviderDto>> GetAllAsync(ProviderFilter filter, CancellationToken ct = default)
    {
        var (items, total) = await _repository.GetAllAsync(filter, ct);

        return new PagedResult<ProviderDto>
        {
            Items = items.Select(p => p.ToDto()).ToList(),
            Total = total,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<ProviderDto> UpdateAsync(Guid id, UpdateProviderRequest request, CancellationToken ct = default)
    {
        var provider = await GetOrFailAsync(id, ct);

        provider.UpdateDetails(request.Name, request.Website, request.Email, request.Country);
        await _repository.SaveChangesAsync(ct);

        return provider.ToDto();
    }

    private async Task<Provider> GetOrFailAsync(Guid id, CancellationToken ct)
    {
        var provider = await _repository.GetByIdAsync(id, ct);
        if (provider is null)
            throw new NotFoundException(nameof(Provider), id);

        return provider;
    }
}