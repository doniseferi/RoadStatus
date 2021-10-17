namespace RoadStatus.Service.ValueObjects
{
    internal class Severity : NotNullEmptyOrWhiteSpacedString
    {
        public Severity(string value) : base(value)
        {
        }
    }
}