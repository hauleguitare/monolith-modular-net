using AutoMapper;
using MonolithModularNET.Extensions.Core.Master;
using MonolithModularNET.Extensions.Shared.Helper;
using MonolithModularNET.Master.Core.Models;

namespace MonolithModularNET.Master.Infrastructure.Mapper;

public class MasterRequestProfile : Profile
{
    public MasterRequestProfile()
    {
        CreateMap_MasterRequest();
    }

    private void CreateMap_MasterRequest()
    {
    }
}

public class MasterResponseProfile : Profile
{
    public MasterResponseProfile()
    {
        CreateMap_MasterResponse();
    }

    private void CreateMap_MasterResponse()
    {

        CreateMap<CompanyTaskSessionProcess, CompanyTaskSessionProcessResponse>()
            .ForMember(dto => dto.StatusName, conf => conf.MapFrom((ol) => StatusHelper.GetName(ol.StatusId)))
            .ForMember(dto => dto.StatusHexColor, conf => conf.MapFrom((ol) => StatusHelper.GetHexColor(ol.StatusId)));
        
        CreateMap<CompanyTaskSession, CompanyTaskSessionResponse>()
            .ForMember(dto => dto.Process, conf => conf.MapFrom((ol) => ol.CompanyTaskSessionProcesses.OrderByDescending(e => e.ActivatedAt).FirstOrDefault()));
        
        CreateMap<CompanyTask, CompanyTaskResponse>()
            .ForMember(dto => dto.CompanyName, conf => conf.MapFrom((ol) => ol.Company.CompanyName))
            .ForMember(dto => dto.Sessions, conf => conf.MapFrom((ol) => ol.CompanyTaskSessions));
    }
}