using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MonolithModularNET.Auth.Core;
using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Extensions.Shared.Models;

namespace MonolithModularNET.User.Queries.GetUserById;

public class GetUserByIdQueryHandler: CqrsQueryHandler<GetUserByIdQuery, UserResponse?>
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(UserManager<AuthUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public override async Task<UserResponse?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null)
        {
            return null;
        }

        return _mapper.Map<UserResponse>(user);
    }
}