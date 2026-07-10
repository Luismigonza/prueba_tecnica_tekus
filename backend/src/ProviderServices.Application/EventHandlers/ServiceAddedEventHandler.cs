using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProviderServices.Application.Interfaces;
using ProviderServices.Application.Settings;
using ProviderServices.Domain.Entities;
using ProviderServices.Domain.Events;

namespace ProviderServices.Application.EventHandlers;

public class ServiceAddedEventHandler : IDomainEventHandler<ServiceAddedEvent>
{
    private readonly IEmailSender _emailSender;
    private readonly IProviderRepository _providerRepository;
    private readonly NotificationSettings _settings;
    private readonly ILogger<ServiceAddedEventHandler> _logger;

    public ServiceAddedEventHandler(
        IEmailSender emailSender,
        IProviderRepository providerRepository,
        IOptions<NotificationSettings> settings,
        ILogger<ServiceAddedEventHandler> logger)
    {
        _emailSender = emailSender;
        _providerRepository = providerRepository;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task HandleAsync(ServiceAddedEvent domainEvent, CancellationToken ct = default)
    {
        try
        {
            var provider = await _providerRepository.GetByIdAsync(domainEvent.ProviderId, ct);
            var providerName = provider?.Name ?? "Unknown provider";

            var subject = $"New service added: {domainEvent.ServiceName}";
            var body = $"Provider '{providerName}' has enabled a new service: " +
                       $"'{domainEvent.ServiceName}' (${domainEvent.HourlyRateUsd}/hour).";

            await _emailSender.SendAsync(_settings.ServiceAddedRecipient, subject, body, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send service-added notification for service {ServiceId}", domainEvent.ServiceId);
        }
    }
}
