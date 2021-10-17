namespace RoadStatus.Service.ValueObjects
{
    internal class Description : NotNullEmptyOrWhiteSpacedString
    {
        public Description(string value) : base(value)
        {
        }
    }
}