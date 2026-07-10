using ProviderServices.Domain.Entities;
using ProviderServices.Domain.Exceptions;

namespace ProviderServices.Domain.Tests;

public class ProviderTests
{
    private static Provider CreateValidProvider() =>
        Provider.Create("900123456", "Importaciones Tekus S.A.", "https://tekus.co", "contact@tekus.co", "Colombia");

    [Fact]
    public void Create_WithValidData_CreatesProvider()
    {
        var provider = CreateValidProvider();

        Assert.Equal("Importaciones Tekus S.A.", provider.Name);
        Assert.Equal("Colombia", provider.Country);
        Assert.NotEqual(Guid.Empty, provider.Id);
    }

    [Fact]
    public void Create_WithoutNit_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(() =>
            Provider.Create("", "Importaciones Tekus S.A.", "https://tekus.co", "contact@tekus.co", "Colombia"));
    }

    [Fact]
    public void Create_WithoutName_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(() =>
            Provider.Create("900123456", "", "https://tekus.co", "contact@tekus.co", "Colombia"));
    }

    [Fact]
    public void UpdateDetails_WithValidData_UpdatesFieldsAndTimestamp()
    {
        var provider = CreateValidProvider();

        provider.UpdateDetails("Nuevo Nombre S.A.", "https://nuevo.co", "nuevo@tekus.co", "Mexico");

        Assert.Equal("Nuevo Nombre S.A.", provider.Name);
        Assert.Equal("Mexico", provider.Country);
        Assert.NotNull(provider.UpdatedAt);
    }

    [Fact]
    public void UpdateDetails_WithoutName_ThrowsDomainException()
    {
        var provider = CreateValidProvider();

        Assert.Throws<DomainException>(() =>
            provider.UpdateDetails("", "https://nuevo.co", "nuevo@tekus.co", "Mexico"));
    }
}