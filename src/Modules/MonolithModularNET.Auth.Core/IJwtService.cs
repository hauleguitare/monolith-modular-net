﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MonolithModularNET.Auth.Core;

public interface IJwtService: IDisposable
{
    public string Encoding(string jti, List<Claim> claims);

    public JwtSecurityToken Decoding(string token);
}