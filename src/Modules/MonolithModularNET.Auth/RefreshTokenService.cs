using System.Text;
using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.Auth;

public class RefreshTokenService: IRefreshTokenService
{
    public TokenResult Encoding(GenerateRefreshTokenOptions options)
    {
        ArgumentNullException.ThrowIfNull(options.Jti);
        ArgumentNullException.ThrowIfNull(options.SecretKey);
        ArgumentNullException.ThrowIfNull(options.ExpiredAt);

        byte[] timeBytes     = BitConverter.GetBytes(options.ExpiredAt.Value.ToBinary());
        byte[] keyBytes      = Guid.Parse(options.Jti).ToByteArray();
        byte[] secretKeyBytes     = System.Text.Encoding.UTF8.GetBytes(options.SecretKey);
        byte[] dataBytes       = new byte[timeBytes.Length + keyBytes.Length + secretKeyBytes.Length];

        Buffer.BlockCopy(timeBytes, 0, dataBytes, 0, timeBytes.Length);
        Buffer.BlockCopy(keyBytes , 0, dataBytes, timeBytes.Length, keyBytes.Length);
        Buffer.BlockCopy(secretKeyBytes , 0, dataBytes, timeBytes.Length + keyBytes.Length, secretKeyBytes.Length);


        var token = Convert.ToBase64String(dataBytes);
        return TokenResult.Success(token);
    }
    
    public TokenResult Decoding(string jti, string secretKey, string token)
    {
        byte[] dataBytes     = Convert.FromBase64String(token);
        byte[] timeBytes     = dataBytes.Take(8).ToArray();
        byte[] keyBytes      = dataBytes.Skip(8).Take(16).ToArray();
        byte[] secretKeyBytes = dataBytes.Skip(24).Take(64).ToArray();

        DateTime when = DateTime.FromBinary(BitConverter.ToInt64(timeBytes, 0));
        if (when < DateTime.UtcNow.AddHours(-24))
        {
            return TokenResult.Failure(new List<AuthError>()
            {
                RefreshTokenDescriber.RefreshTokenExpiredTime()
            });
        }
    
        Guid gKey = new Guid(keyBytes);
        if (gKey.ToString() != jti)
        {
            return TokenResult.Failure(new List<AuthError>()
            {
                RefreshTokenDescriber.RefreshTokenJtiNotMatch()
            });
        }

        if (System.Text.Encoding.UTF8.GetString(secretKeyBytes) != secretKey)
        {
            return TokenResult.Failure(new List<AuthError>()
            {
                RefreshTokenDescriber.RefreshTokenSecretKeyNotMatch()
            });
        }

        return TokenResult.Success();
    }
}