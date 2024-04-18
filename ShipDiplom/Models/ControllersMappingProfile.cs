using AutoMapper;
using JetBrains.Annotations;

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
    }
    private void MapResponses()
    {

    }
}
