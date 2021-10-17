namespace RoadStatus.Service.ValueObjects
{
    internal class RoadId : NotNullEmptyOrWhiteSpacedString
    {
        public RoadId(string value) : base(value)
        {
        }
    }
}