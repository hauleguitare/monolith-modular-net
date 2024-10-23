using System.Text;
using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.Auth;

public class RefreshTokenService: IRefreshTokenService
{
    public TokenResult GenerateRefreshToken(string jti, string secretKey, DateTime expiredAt)
    {
        byte[] timeBytes     = BitConverter.GetBytes(expiredAt.ToBinary());
        byte[] keyBytes      = Guid.Parse(jti).ToByteArray();
        byte[] secretKeyBytes     = Encoding.UTF8.GetBytes(secretKey);
        byte[] dataBytes       = new byte[timeBytes.Length + keyBytes.Length + secretKeyBytes.Length];

        Buffer.BlockCopy(timeBytes, 0, dataBytes, 0, timeBytes.Length);
        Buffer.BlockCopy(keyBytes , 0, dataBytes, timeBytes.Length, keyBytes.Length);
        Buffer.BlockCopy(secretKeyBytes , 0, dataBytes, timeBytes.Length + keyBytes.Length, secretKeyBytes.Length);

        return TokenResult.Success(Convert.ToBase64String(dataBytes.ToArray()));
    }
    
    public TokenResult ValidateRefreshToken(string jti, string secretKey, string token)
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

        if (Encoding.UTF8.GetString(secretKeyBytes) != secretKey)
        {
            return TokenResult.Failure(new List<AuthError>()
            {
                RefreshTokenDescriber.RefreshTokenSecretKeyNotMatch()
            });
        }

        return TokenResult.Success();
    }
}