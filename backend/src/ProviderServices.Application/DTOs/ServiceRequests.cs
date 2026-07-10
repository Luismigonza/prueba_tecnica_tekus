namespace ProviderServices.Application.DTOs;

public class CreateServiceRequest
{
    public string Name { get; set; } = default!;
    public decimal HourlyRateUsd { get; set; }
}