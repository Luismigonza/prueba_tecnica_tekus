using ProviderServices.Domain.Events;
using ProviderServices.Domain.Exceptions;

namespace ProviderServices.Domain.Entities;

public class Service
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public decimal HourlyRateUsd { get; private set; }
    public Guid ProviderId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private Service()
    {
    }

    private Service(string name, decimal hourlyRateUsd, Guid providerId)
    {
        Id = Guid.NewGuid();
        Name = name;
        HourlyRateUsd = hourlyRateUsd;
        ProviderId = providerId;
        CreatedAt = DateTime.UtcNow;
    }

    public static Service Create(string name, decimal hourlyRateUsd, Guid providerId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Service name is required.");

        if (hourlyRateUsd <= 0)
            throw new DomainException("Hourly rate must be greater than zero.");

        var service = new Service(name, hourlyRateUsd, providerId);
        service._domainEvents.Add(new ServiceAddedEvent(service.Id, service.Name, service.ProviderId, service.HourlyRateUsd));

        return service;
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}