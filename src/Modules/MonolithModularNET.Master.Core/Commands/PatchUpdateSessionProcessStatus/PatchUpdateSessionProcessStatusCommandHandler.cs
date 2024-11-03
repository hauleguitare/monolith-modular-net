using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MonolithModularNET.Extensions.Core.Master;
using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Master.Core.Models;
using MonolithModularNET.Master.Core.Repositories;

namespace MonolithModularNET.Master.Core.Commands.PatchUpdateSessionProcessStatus;

public class PatchUpdateSessionProcessStatusCommandHandler(IMasterReadonlyRepository<CompanyTaskSession> sessionReadonlyRepo, IMasterUnitOfWork unitOfWork, IMapper mapper)
    : CqrsCommandHandler<PatchUpdateSessionProcessStatusCommand, CompanyTaskSessionResponse>
{

    public override async Task<CqrsResult<CompanyTaskSessionResponse>> Handle(PatchUpdateSessionProcessStatusCommand request, CancellationToken cancellationToken)
    {
        var describer = new MasterErrorDescriber();
        var session = await sessionReadonlyRepo.AsQueryable()
            .Include(e => e.CompanyTaskSessionProcesses)
            .Where(e => e.Email == request.Email)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (session is null)
        {
            return CqrsResult<CompanyTaskSessionResponse>.Failure(describer.TaskSessionNotFound());
        }
        
        session.CompanyTaskSessionProcesses.Add(new CompanyTaskSessionProcess()
        {
            StatusId = request.StatusId,
            ActivatedAt = request.ActivatedAt
        });

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var resp = mapper.Map<CompanyTaskSessionResponse>(session);

        return CqrsResult<CompanyTaskSessionResponse>.Succeed(resp);
    }
}