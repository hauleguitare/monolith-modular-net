using AutoMapper;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Shared.Models;

namespace MonolithModularNET.User.Infrastructure.Mapper;

public class UserRequestProfile : Profile
{
    public UserRequestProfile()
    {
        CreateMap_UserRequest();
    }

    private void CreateMap_UserRequest()
    {
    }
}

public class UserResponseProfile : Profile
{
    public UserResponseProfile()
    {
        CreateMap_UserResponse();
    }

    private void CreateMap_UserResponse()
    {
        CreateMap<AuthUser, UserResponse>();
    }
}