namespace ProviderServices.Application.DTOs;

public class ServiceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal HourlyRateUsd { get; set; }
    public Guid ProviderId { get; set; }
    public DateTime CreatedAt { get; set; }
}