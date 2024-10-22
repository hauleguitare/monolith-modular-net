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
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public string Encrypt(object data)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(Issuer,
            Issuer,
            new List<Claim>()
            {
                new Claim("user_id", data.ToString())
            },
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}