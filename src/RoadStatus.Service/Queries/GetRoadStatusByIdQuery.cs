using System;
using RoadStatus.Service.ValueObjects;

namespace RoadStatus.Service.Queries
{
    internal class GetRoadStatusByIdQuery
    {
        public GetRoadStatusByIdQuery(RoadId roadId)
        {
            RoadId = roadId ?? throw new ArgumentNullException(nameof(roadId));
        }

        public RoadId RoadId { get; }
    }
}