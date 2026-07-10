namespace ProviderServices.Application.DTOs;

public class CreateProviderRequest
{
    public string Nit { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Website { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Country { get; set; } = default!;
}

public class UpdateProviderRequest
{
    public string Name { get; set; } = default!;
    public string Website { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Country { get; set; } = default!;
}