using Microsoft.Extensions.Options;
using ProviderServices.Application.DTOs;
using ProviderServices.Application.Exceptions;
using ProviderServices.Application.Interfaces;
using ProviderServices.Application.Settings;

namespace ProviderServices.Application.Services;

public class AuthAppService : IAuthAppService
{
    private readonly DefaultUserSettings _defaultUser;
    private readonly ITokenGenerator _tokenGenerator;

    public AuthAppService(IOptions<DefaultUserSettings> defaultUser, ITokenGenerator tokenGenerator)
    {
        _defaultUser = defaultUser.Value;
        _tokenGenerator = tokenGenerator;
    }

    public LoginResponse Login(LoginRequest request)
    {
        var isValid = request.Username == _defaultUser.Username && request.Password == _defaultUser.Password;
        if (!isValid)
            throw new UnauthorizedException("Invalid username or password.");

        var (token, expiresAt) = _tokenGenerator.GenerateToken(request.Username);

        return new LoginResponse { Token = token, ExpiresAt = expiresAt };
    }
}
