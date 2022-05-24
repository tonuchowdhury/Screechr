using System.Security.Claims;

namespace Screechr.Service.API
{
    public interface ITokenService
    {
        string GenerateToken(double expiresInMin, IEnumerable<Claim>? claims, string? audience, string? issuer, out DateTime expiresIn);
        bool ValidateAuthorizationToken(string? token, out string? userName, out string? errorMessage);
    }
}