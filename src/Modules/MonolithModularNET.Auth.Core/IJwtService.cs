using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MonolithModularNET.Auth.Core;

public interface IJwtService: IDisposable
{
    public string Encoding(CreateJwtTokenOptions tokenOptions);

    public JwtSecurityToken Decoding(string token);
}