using Microsoft.AspNetCore.Mvc;
using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController: ControllerBase
{
    private readonly ISignUpService<AuthUser, AuthRole> _signUpService;

    public AuthController(ISignUpService<AuthUser, AuthRole> signUpService)
    {
        _signUpService = signUpService;
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
    {
       await _signUpService.SignUpAsync(request);

       return Ok();
    }
}