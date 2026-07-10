namespace ProviderServices.Application.DTOs;

public class SummaryDto
{
    public List<CountByCountryDto> ProvidersByCountry { get; set; } = new();
    public List<CountByCountryDto> ServicesByCountry { get; set; } = new();
}

public class CountByCountryDto
{
    public string Country { get; set; } = default!;
    public int Count { get; set; }
}