using System.Security.Claims;

namespace MonolithModularNET.Auth.Core;

public interface IJwtService: IDisposable
{
    public string Encrypt(List<Claim> claims);
}