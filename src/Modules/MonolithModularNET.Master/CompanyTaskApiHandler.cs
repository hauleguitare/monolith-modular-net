using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonolithModularNET.Extensions.Shared.Models;
using MonolithModularNET.Master.Core.Commands.AddSessionProcess;
using MonolithModularNET.Master.Core.Queries;
using MonolithModularNET.Master.Core.Queries.CheckSession;
using MonolithModularNET.Master.Core.Queries.GetCompanyTask;

namespace MonolithModularNET.Master;

public class CompanyTaskApiHandler
{
    public async Task<IResult> GetAsync([FromServices] ISender sender)
    {
        var result = await sender.Send(new GetCompanyTaskQuery());
        
        return Results.Ok(ApiResponse.Success(result));
    }

    public async Task<IResult> CheckSessionAsync([FromBody] CheckSessionQuery query, [FromServices] ISender sender)
    {
        var result = await sender.Send(query);

        return Results.Ok(ApiResponse.Success(result));
    }

    public async Task<IResult> AddSessionAsync([FromBody] AddSessionProcessCommand command, [FromServices] ISender sender)
    {
        var result = await sender.Send(command);
        
        return Results.Ok(ApiResponse.Success(result));
    }
}