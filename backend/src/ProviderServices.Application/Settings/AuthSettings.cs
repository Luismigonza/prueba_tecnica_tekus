namespace ProviderServices.Application.Settings;

public class JwtSettings
{
    public string Key { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public int ExpiryMinutes { get; set; } = 60;
}

public class DefaultUserSettings
{
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}
