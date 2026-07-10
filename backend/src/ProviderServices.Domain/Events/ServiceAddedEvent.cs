namespace ProviderServices.Domain.Events;

public class ServiceAddedEvent : IDomainEvent
{
    public Guid ServiceId { get; }
    public string ServiceName { get; }
    public Guid ProviderId { get; }
    public decimal HourlyRateUsd { get; }
    public DateTime OccurredOn { get; }

    public ServiceAddedEvent(Guid serviceId, string serviceName, Guid providerId, decimal hourlyRateUsd)
    {
        ServiceId = serviceId;
        ServiceName = serviceName;
        ProviderId = providerId;
        HourlyRateUsd = hourlyRateUsd;
        OccurredOn = DateTime.UtcNow;
    }
}