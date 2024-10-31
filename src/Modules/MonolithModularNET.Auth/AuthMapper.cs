using AutoMapper;
using MonolithModularNET.Auth.Core;

namespace MonolithModularNET.Auth;

public class AuthRequestProfile : Profile
{
    public AuthRequestProfile()
    {
        CreateMap_AuthV1ClassicTokenCreateRequest();
    }

    private void CreateMap_AuthV1ClassicTokenCreateRequest()
    {
        CreateMap<AuthV1ClassicTokenCreateRequest, AuthV1ClassicToken>();
        CreateMap<AuthV1ClassicTokenClaimRequest, AuthV1ClassicTokenClaim>();
    }
}

public class AuthResponseProfile : Profile
{
    public AuthResponseProfile()
    {
        CreateMap_AuthV1ClassicTokenResponse();
    }

    private void CreateMap_AuthV1ClassicTokenResponse()
    {
        CreateMap<AuthV1ClassicToken, AuthV1ClassicTokenResponse>();
        CreateMap<AuthV1ClassicTokenClaim, AuthV1ClassicTokenClaimResponse>();
    }
}