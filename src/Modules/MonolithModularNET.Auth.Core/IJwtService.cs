namespace MonolithModularNET.Auth.Core;

public interface IJwtService: IDisposable
{
    
    public string Encrypt(object data);
}