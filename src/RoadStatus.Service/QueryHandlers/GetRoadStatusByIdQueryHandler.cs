using System;
using System.Threading.Tasks;
using LanguageExt;
using RoadStatus.Service.Entities;
using RoadStatus.Service.Repository;
using RoadStatus.Service.ValueObjects;

namespace RoadStatus.Service.QueryHandlers
{
    internal class GetRoadStatusByIdQueryHandler : IGetRoadStatusByIdQueryHandler
    {
        private readonly IRoadRepository _roadRepository;

        public GetRoadStatusByIdQueryHandler(IRoadRepository roadRepository)
        {
            _roadRepository = roadRepository ?? throw new ArgumentNullException(nameof(roadRepository));
        }

        public async Task<Option<Road>> HandleAsync(RoadId roadId) => roadId != null
            ? await _roadRepository.GetByIdAsync(roadId)
            : throw new ArgumentNullException(nameof(roadId));
    }
}