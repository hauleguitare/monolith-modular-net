using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MonolithModularNET.Extensions.Core.Master;
using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Extensions.Shared.Services;
using MonolithModularNET.Master.Core.Models;
using MonolithModularNET.Master.Core.Repositories;

namespace MonolithModularNET.Master.Core.Queries.GetCompanyTask;

public class GetCompanyTaskQueryHandler(IMasterReadonlyRepository<CompanyTask> companyTaskRepo, IMapper mapper, ICurrentUserService currentUserService)
    : CqrsQueryHandler<GetCompanyTaskQuery, ICollection<CompanyTaskResponse>>
{

    public override async Task<ICollection<CompanyTaskResponse>> Handle(GetCompanyTaskQuery request, CancellationToken cancellationToken)
    {
        var tasks = await companyTaskRepo.AsNoTracking()
            .Select(e => new CompanyTask()
            {
                Id = e.Id,
                Type = e.Type,
                TaskName = e.TaskName,
                Description = e.Description,
                CompanyId = e.CompanyId,
                Company = e.Company,
                CompanyTaskSessions = e.CompanyTaskSessions.Where(e => e.CreatedBy == currentUserService.UserId).ToList()
            })
            .ProjectTo<CompanyTaskResponse>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        
        return tasks;
    }
}