using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.Auth;

public class JwtService: IJwtService
{
    private readonly AuthJwtTokenOptions _options;

    public JwtService(AuthJwtTokenOptions options)
    {
        _options = options;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public string Encrypt(List<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expiredAt = DateTime.UtcNow.AddMinutes(_options.ExpiresIn);

        var token = new JwtSecurityToken(_options.Issuer,
            _options.Issuer,
            claims,
            expires: expiredAt,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}