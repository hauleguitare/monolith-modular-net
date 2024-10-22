using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.Auth;

public class JwtService: IJwtService
{
    public string SecretKey = "@32jseaX#@!XDAS3213123!312345345gdfgdfsdadas34312dascghgffsfsd@#dasd";
    public string Issuer = "http://localhost:5430";
    public int ExpiresIn = 120;
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public string Encrypt(List<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expiredAt = DateTime.UtcNow.AddMinutes(ExpiresIn);

        var token = new JwtSecurityToken(Issuer,
            Issuer,
            claims,
            expires: expiredAt,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}