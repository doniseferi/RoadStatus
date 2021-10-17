namespace RoadStatus.Service.ValueObjects
{
    internal class Name : NotNullEmptyOrWhiteSpacedString
    {
        public Name(string value) : base(value)
        {
        }
    }
}