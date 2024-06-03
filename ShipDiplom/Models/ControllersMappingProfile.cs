using AutoMapper;
using JetBrains.Annotations;
using ShipDiplom.Models.Dto.Request;
using ShipDiplom.Models.Entities;

namespace ShipDiplom.Models;

public class ControllersMappingProfile : Profile
{
    [UsedImplicitly]
    public ControllersMappingProfile()
    {
        MapRequests();
        MapResponses();
    }
    private void MapRequests()
    {
        CreateMap<ShipUpdateDto, Ship>()
           .ForMember(src => src.Pier, opt => opt.Ignore());

        CreateMap<Ship, ShipUpdateDto>();
    }
    private void MapResponses()
    {

    }
}
