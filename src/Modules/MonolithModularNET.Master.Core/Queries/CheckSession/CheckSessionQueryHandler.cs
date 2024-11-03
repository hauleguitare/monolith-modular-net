using Microsoft.EntityFrameworkCore;
using MonolithModularNET.Extensions.Core.Master;
using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Master.Core.Repositories;

namespace MonolithModularNET.Master.Core.Queries.CheckSession;

public class CheckSessionQueryHandler(IMasterReadonlyRepository<CompanyTaskSession> sessionRepo)
    : CqrsQueryHandler<CheckSessionQuery, bool>
{
    public override async Task<bool> Handle(CheckSessionQuery request, CancellationToken cancellationToken)
    {
        var latestProcess = await sessionRepo.AsNoTracking().Where(e => e.Email == request.Email)
            .SelectMany(e => e.CompanyTaskSessionProcesses)
            .OrderByDescending(e => e.ActivatedAt).FirstOrDefaultAsync();

        if (latestProcess is null)
        {
            return true;
        }
        return latestProcess.StatusId == 0;
    }
}