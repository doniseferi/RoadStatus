using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LanguageExt;
using RoadStatus.Service.Entities;
using RoadStatus.Service.ValueObjects;

[assembly: InternalsVisibleTo("RoadStatus.Console")]
namespace RoadStatus.Service.QueryHandlers
{
    internal interface IGetRoadStatusByIdQueryHandler
    {
        Task<Option<Road>> HandleAsync(RoadId roadId);
    }
}