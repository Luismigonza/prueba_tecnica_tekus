using ProviderServices.Application.DTOs;
using ProviderServices.Application.Exceptions;
using ProviderServices.Application.Interfaces;
using ProviderServices.Application.Mapping;
using ProviderServices.Domain.Entities;

namespace ProviderServices.Application.Services;

public class ServicesAppService : IServicesAppService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IProviderRepository _providerRepository;
    private readonly IDomainEventDispatcher _eventDispatcher;

    public ServicesAppService(
        IServiceRepository serviceRepository,
        IProviderRepository providerRepository,
        IDomainEventDispatcher eventDispatcher)
    {
        _serviceRepository = serviceRepository;
        _providerRepository = providerRepository;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<ServiceDto> CreateAsync(Guid providerId, CreateServiceRequest request, CancellationToken ct = default)
    {
        var provider = await _providerRepository.GetByIdAsync(providerId, ct);
        if (provider is null)
            throw new NotFoundException(nameof(Provider), providerId);

        var service = Service.Create(request.Name, request.HourlyRateUsd, providerId);

        await _serviceRepository.AddAsync(service, ct);
        await _serviceRepository.SaveChangesAsync(ct);

        await _eventDispatcher.DispatchAsync(service.DomainEvents, ct);
        service.ClearDomainEvents();

        return service.ToDto();
    }

    public async Task<PagedResult<ServiceDto>> GetByProviderIdAsync(Guid providerId, ServiceFilter filter, CancellationToken ct = default)
    {
        var (items, total) = await _serviceRepository.GetByProviderIdAsync(providerId, filter, ct);

        return new PagedResult<ServiceDto>
        {
            Items = items.Select(s => s.ToDto()).ToList(),
            Total = total,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }
}