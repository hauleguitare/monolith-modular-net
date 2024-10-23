using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

    public string Encoding(string jti, List<Claim> claims)
    {
        var overrideClaims = new List<Claim>() { new Claim(JwtRegisteredClaimNames.Jti, jti) };
        overrideClaims.AddRange(claims);
        
        var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_options.SecretKey!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expiredAt = DateTime.UtcNow.AddMinutes(_options.ExpiresIn);

        var token = new JwtSecurityToken(_options.Issuer,
            _options.Issuer,
            overrideClaims,
            expires: expiredAt,
            signingCredentials: credentials);
        
        

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public JwtSecurityToken Decoding(string token)
    {
         return new JwtSecurityTokenHandler().ReadJwtToken(token);
    }
}