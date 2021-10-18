using LanguageExt;
using RoadStatus.Data.Dto;
using RoadStatus.Service.Entities;
using RoadStatus.Service.ValueObjects;

namespace RoadStatus.Data.Mapper
{
    internal class DtoToDomainMapper : IDtoToDomainMapper
    {
        public Option<Road> Map(Option<RoadResponse> roadResponse) =>
            roadResponse.Match(
                Some: x => new Some<Road>(
                    new Road(
                        new RoadId(x.Id),
                        new Name(x.DisplayName),
                        new Status(
                            new Severity(x.StatusSeverity),
                            new Description(x.StatusSeverityDescription)))),
                None: Option<Road>.None);
    }
}