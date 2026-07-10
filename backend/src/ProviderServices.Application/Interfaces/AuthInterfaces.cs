using ProviderServices.Application.DTOs;

namespace ProviderServices.Application.Interfaces;

public interface ITokenGenerator
{
    (string Token, DateTime ExpiresAt) GenerateToken(string username);
}

public interface IAuthAppService
{
    LoginResponse Login(LoginRequest request);
}
