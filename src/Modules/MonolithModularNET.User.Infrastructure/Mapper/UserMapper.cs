using AutoMapper;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Shared.Models;
using MonolithModularNET.User.Core.Commands.PatchUpdateUser;
using MonolithModularNET.User.Models;

namespace MonolithModularNET.User.Infrastructure.Mapper;

public class UserRequestProfile : Profile
{
    public UserRequestProfile()
    {
        CreateMap_UserRequest();
    }

    private void CreateMap_UserRequest()
    {
        CreateMap<PatchUpdateUserRequest, PatchUpdateUserCommand>();
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