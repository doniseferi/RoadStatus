using System;
using LanguageExt;

namespace RoadStatus.Service.ValueObjects
{
    internal class NotNullEmptyOrWhiteSpacedString : NewType<NotNullEmptyOrWhiteSpacedString, string>
    {
        public NotNullEmptyOrWhiteSpacedString(string value) : base(value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(nameof(value));
            }
        }
    }
}