using JwtDemonstration.Models;
using JwtDemonstration.Models.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JwtDemonstration.Services;

public interface ITokenService
{
    TokenResponse CreateAccessToken(User user);
}

public class TokenService : ITokenService
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly SymmetricSecurityKey _signingKey;
    private readonly int _lifetimeMinutes;

    public TokenService(string issuer, string audience, SymmetricSecurityKey signingKey, int lifetimeMinutes)
    {
        _issuer = issuer;
        _audience = audience;
        _signingKey = signingKey;
        _lifetimeMinutes = lifetimeMinutes;
    }

    public TokenResponse CreateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username)
        };
        claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var creds = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_lifetimeMinutes),
            signingCredentials: creds
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        return new TokenResponse { AccessToken = token, ExpiresOn = _lifetimeMinutes * 60 };
    }
}
