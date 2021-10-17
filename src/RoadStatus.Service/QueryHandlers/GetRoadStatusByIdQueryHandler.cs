using System.Threading.Tasks;
using LanguageExt;
using RoadStatus.Service.Entities;
using RoadStatus.Service.ValueObjects;

namespace RoadStatus.Service.QueryHandlers
{
    internal class GetRoadStatusByIdQueryHandler : IGetRoadStatusByIdQueryHandler
    {
        public Task<Option<Road>> HandleAsync(RoadId roadId)
        {
            throw new System.NotImplementedException();
        }
    }
}