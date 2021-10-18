using LanguageExt;
using RoadStatus.Data.Dto;
using RoadStatus.Service.Entities;

namespace RoadStatus.Data.Mapper
{
    internal interface IDtoToDomainMapper
    {
        Option<Road> Map(Option<RoadResponse> roadResponse);
    }
}