using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.Auth;

public class JwtService: IJwtService
{
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public string Encoding(CreateJwtTokenOptions tokenOptions)
    {
        ArgumentNullException.ThrowIfNull(tokenOptions.Id);
        ArgumentNullException.ThrowIfNull(tokenOptions.SecretKey);
        ArgumentNullException.ThrowIfNull(tokenOptions.ExpiredAt);

        var overrideClaims = new List<Claim>() { new (JwtRegisteredClaimNames.Jti, tokenOptions.Id) };
        overrideClaims.AddRange(tokenOptions.Claims);
        
        var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenOptions.SecretKey!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(tokenOptions.Issuer,
            tokenOptions.Audience,
            overrideClaims,
            expires: tokenOptions.ExpiredAt,
            signingCredentials: credentials);
        
        

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public JwtSecurityToken Decoding(string token)
    {
         return new JwtSecurityTokenHandler().ReadJwtToken(token);
    }
}