using ProviderServices.Domain.Exceptions;

namespace ProviderServices.Domain.Entities;

public class Provider
{
    public Guid Id { get; private set; }
    public string Nit { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string Website { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string Country { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Provider()
    {
    }

    private Provider(string nit, string name, string website, string email, string country)
    {
        Id = Guid.NewGuid();
        Nit = nit;
        Name = name;
        Website = website;
        Email = email;
        Country = country;
        CreatedAt = DateTime.UtcNow;
    }

    public static Provider Create(string nit, string name, string website, string email, string country)
    {
        if (string.IsNullOrWhiteSpace(nit))
            throw new DomainException("Nit is required.");

        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name is required.");

        return new Provider(nit, name, website, email, country);
    }

    public void UpdateDetails(string name, string website, string email, string country)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name is required.");

        Name = name;
        Website = website;
        Email = email;
        Country = country;
        UpdatedAt = DateTime.UtcNow;
    }
}