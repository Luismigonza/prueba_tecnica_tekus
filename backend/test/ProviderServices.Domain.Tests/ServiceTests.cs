using ProviderServices.Domain.Entities;
using ProviderServices.Domain.Events;
using ProviderServices.Domain.Exceptions;

namespace ProviderServices.Domain.Tests;

public class ServiceTests
{
    private static readonly Guid ProviderId = Guid.NewGuid();

    [Fact]
    public void Create_WithValidData_CreatesService()
    {
        var service = Service.Create("Descarga espacial de contenidos", 45.50m, ProviderId);

        Assert.Equal("Descarga espacial de contenidos", service.Name);
        Assert.Equal(45.50m, service.HourlyRateUsd);
        Assert.Equal(ProviderId, service.ProviderId);
    }

    [Fact]
    public void Create_WithZeroHourlyRate_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(() => Service.Create("Servicio X", 0m, ProviderId));
    }

    [Fact]
    public void Create_WithNegativeHourlyRate_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(() => Service.Create("Servicio X", -10m, ProviderId));
    }

    [Fact]
    public void Create_WithoutName_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(() => Service.Create("", 45.50m, ProviderId));
    }

    [Fact]
    public void Create_RaisesServiceAddedEvent()
    {
        var service = Service.Create("Desaparicion forzada de bytes", 30m, ProviderId);

        var domainEvent = Assert.Single(service.DomainEvents);
        var serviceAddedEvent = Assert.IsType<ServiceAddedEvent>(domainEvent);
        Assert.Equal(service.Id, serviceAddedEvent.ServiceId);
        Assert.Equal(ProviderId, serviceAddedEvent.ProviderId);
    }

    [Fact]
    public void ClearDomainEvents_RemovesPendingEvents()
    {
        var service = Service.Create("Servicio X", 30m, ProviderId);

        service.ClearDomainEvents();

        Assert.Empty(service.DomainEvents);
    }
}