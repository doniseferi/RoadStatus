using System;
using RoadStatus.Service.ValueObjects;

namespace RoadStatus.Service.Entities
{
    internal sealed class Road
    {
        public Road(RoadId id, Name name,Status status)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Status = status ?? throw new ArgumentNullException(nameof(status));
        }

        public RoadId Id { get; }
        public Name Name { get; set; }
        public Status Status { get; }
    }
}