using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LanguageExt;
using RoadStatus.Service.Entities;
using RoadStatus.Service.ValueObjects;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace RoadStatus.Service.Repository
{
    internal interface IRoadRepository
    {
        Task<Option<Road>> GetByIdAsync(RoadId roadId);
    }
}