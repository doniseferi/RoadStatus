using System;

namespace RoadStatus.Service.ValueObjects
{
    internal class Status
    {
        public Status(Severity severity, Description description)
        {
            Severity = severity ?? throw new ArgumentNullException(nameof(severity));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public Severity Severity { get; }
        public Description Description { get; }
    }
}