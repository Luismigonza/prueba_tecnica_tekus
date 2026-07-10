using ProviderServices.Application.DTOs;
using ProviderServices.Domain.Entities;

namespace ProviderServices.Application.Mapping;

public static class MappingExtensions
{
    public static ProviderDto ToDto(this Provider provider) => new()
    {
        Id = provider.Id,
        Nit = provider.Nit,
        Name = provider.Name,
        Website = provider.Website,
        Email = provider.Email,
        Country = provider.Country,
        CreatedAt = provider.CreatedAt,
        UpdatedAt = provider.UpdatedAt
    };

    public static ServiceDto ToDto(this Service service) => new()
    {
        Id = service.Id,
        Name = service.Name,
        HourlyRateUsd = service.HourlyRateUsd,
        ProviderId = service.ProviderId,
        CreatedAt = service.CreatedAt
    };
}