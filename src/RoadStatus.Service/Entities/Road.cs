using System;
using RoadStatus.Service.ValueObjects;

namespace RoadStatus.Service.Entities
{
    internal sealed class Road
    {
        public Road(RoadId id, Status status)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Status = status ?? throw new ArgumentNullException(nameof(status));
        }

        public RoadId Id { get; }
        public Status Status { get; }
    }
}