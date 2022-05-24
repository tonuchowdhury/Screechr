using Microsoft.IdentityModel.Tokens;
using Screechr.Service.API;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Screechr.Service
{
    public class TokenService : ITokenService
    {
        private const string TokenSecretKey = "$$TokenSecret12345##";
        private const int TokenExpiresInMinutes = 1440; // one day
        private const string TokenType = "Beare";

        public string GenerateToken(double expiresInMin, IEnumerable<Claim>? claims, string? audience, string? issuer, out DateTime expiresIn)
        {
            var key = Encoding.ASCII.GetBytes(TokenSecretKey ?? string.Empty);
            var securityKey = new SymmetricSecurityKey(key);

            expiresIn = DateTime.UtcNow.AddMinutes(expiresInMin);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiresIn,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            descriptor.Audience = audience;
            descriptor.Issuer = issuer;

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }

        public bool ValidateAuthorizationToken(string? token, out string? userName, out string? errorMessage)
        {
            userName = null;
            errorMessage = null;

            if (token == null || !token.StartsWith(TokenType))
            {
                errorMessage = "Invalid token";
                return false;
            }

            token = token.Replace(TokenType, string.Empty).TrimStart();
            var identity = GetIdentityFromToken(token, out var expirationDate, out var securityToken,
                out var claims, out var claimsPrincipal);

            if (identity == null)
            {
                errorMessage = "Unauthorized";
                return false;
            }

            var userNameClaim = identity.FindFirst(ClaimTypes.Name);

            if (!expirationDate.HasValue || expirationDate.Value < DateTime.UtcNow)
            {
                errorMessage = "Token expired";
                return false;
            }

            if (string.IsNullOrEmpty(userNameClaim?.Value))
            {
                errorMessage = "Unauthorized";
                return false;
            }

            userName = userNameClaim.Value;

            if (string.IsNullOrEmpty(userName))
            {
                errorMessage = "Unauthorized";
                return false;
            }

            return true;
        }

        private ClaimsPrincipal? GetPrincipal(string? token, ref DateTime? expirationDate, ref SecurityToken? securityToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;

                expirationDate = jwtToken.ValidTo;
                byte[] key = Encoding.ASCII.GetBytes(TokenSecretKey);

                var parameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                var principal = tokenHandler.ValidateToken(token, parameters, out securityToken);

                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private ClaimsIdentity? GetIdentityFromToken(string? token, out DateTime? expirationDate,
            out SecurityToken? securityToken, out IEnumerable<Claim>? claims, out ClaimsPrincipal? claimsPrincipal)
        {
            expirationDate = null;
            securityToken = null;
            claims = null;
            claimsPrincipal = GetPrincipal(token, ref expirationDate, ref securityToken);
            if (claimsPrincipal == null)
                return null;

            try
            {
                var identity = claimsPrincipal.Identity as ClaimsIdentity;
                claims = claimsPrincipal.Claims;

                return identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }
    }
}