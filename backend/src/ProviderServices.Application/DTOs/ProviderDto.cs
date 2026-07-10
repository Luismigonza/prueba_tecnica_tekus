namespace ProviderServices.Application.DTOs;

public class ProviderDto
{
    public Guid Id { get; set; }
    public string Nit { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Website { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Country { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}