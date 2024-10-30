using System.Security.Cryptography;
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
        
        using (SHA256 mySha256 = SHA256.Create())
        {
            byte[] timeBytes     = BitConverter.GetBytes(options.ExpiredAt.Value.ToBinary());
            byte[] keyBytes      = Guid.Parse(options.Jti).ToByteArray();
            byte[] secretKeyBytes     = System.Text.Encoding.UTF8.GetBytes(GetHash(mySha256, options.SecretKey));
            byte[] dataBytes       = new byte[timeBytes.Length + keyBytes.Length + secretKeyBytes.Length];

            Buffer.BlockCopy(timeBytes, 0, dataBytes, 0, timeBytes.Length);
            Buffer.BlockCopy(keyBytes , 0, dataBytes, timeBytes.Length, keyBytes.Length);
            Buffer.BlockCopy(secretKeyBytes , 0, dataBytes, timeBytes.Length + keyBytes.Length, secretKeyBytes.Length);


            var token = Convert.ToBase64String(dataBytes);
            return TokenResult.Success(token);
        }
    }

    private string GetHash(HashAlgorithm hashAlgorithm, string input)
    {
        // Convert the input string to a byte array and compute the hash.
        byte[] data = hashAlgorithm.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        var sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }
    
    // Verify a hash against a string.
    private bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
    {
        // Hash the input.
        var hashOfInput = GetHash(hashAlgorithm, input);

        // Create a StringComparer an compare the hashes.
        StringComparer comparer = StringComparer.OrdinalIgnoreCase;

        return comparer.Compare(hashOfInput, hash) == 0;
    }
    
    public TokenResult Decoding(string jti, string secretKey, string token)
    {
        byte[] dataBytes     = Convert.FromBase64String(token);
        byte[] timeBytes     = dataBytes.Take(8).ToArray();
        byte[] keyBytes      = dataBytes.Skip(8).Take(16).ToArray();
        
        
        byte[] hashedSecretKeyBytes = dataBytes.Skip(24).Take(dataBytes.Length - 1).ToArray();

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

        using (SHA256 mySha256 = SHA256.Create())
        {
            var isVerifiedHashed = VerifyHash(mySha256, secretKey, System.Text.Encoding.UTF8.GetString(hashedSecretKeyBytes));

            if (!isVerifiedHashed)
            {
                return TokenResult.Failure(new List<AuthError>()
                {
                   RefreshTokenDescriber.RefreshTokenSecretKeyNotMatch()
                });
            }
        }

        return TokenResult.Success();
    }
}