using AutoMapper;
using MonolithModularNET.Extensions.Core.Master;
using MonolithModularNET.Extensions.Shared.Cqrs;
using MonolithModularNET.Master.Core.Models;
using MonolithModularNET.Master.Core.Repositories;

namespace MonolithModularNET.Master.Core.Commands.AddSessionProcess;

public class AddSessionProcessCommandHandler(IMasterReadonlyRepository<CompanyTaskSession> sessionRepo, IMasterWriteableRepository<CompanyTaskSession> writeableSessionRepository, IMasterUnitOfWork unitOfWork, IMapper mapper)
    : CqrsCommandHandler<AddSessionProcessCommand, CompanyTaskSessionResponse?>
{
    public override Task<CqrsResult<CompanyTaskSessionResponse?>> Handle(AddSessionProcessCommand request, CancellationToken cancellationToken)
    {
        var newSession = new CompanyTaskSession()
        {
            CompanyTaskId = request.CompanyTaskId,
            Email = request.Email,
            Password = request.Password,
            CompanyTaskSessionProcesses = new List<CompanyTaskSessionProcess>()
            {
                new CompanyTaskSessionProcess()
                {
                    StatusId = request.StatusId,
                    ActivatedAt = DateTime.Now,
                }
            }
        };
        
        writeableSessionRepository.Add(newSession);

        unitOfWork.SaveChanges();

        return Task.FromResult(
            CqrsResult<CompanyTaskSessionResponse?>.Succeed(mapper.Map<CompanyTaskSessionResponse>(newSession))
            );
    }
}