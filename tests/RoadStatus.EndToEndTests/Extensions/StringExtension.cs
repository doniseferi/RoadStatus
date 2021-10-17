using System.Globalization;

namespace RoadStatus.EndToEndTests.Extensions
{
    internal static class StringExtension
    {
        public static string ToTitledCase(this string str)
            => string.IsNullOrWhiteSpace(str)
                ? str
                : new CultureInfo("en-GB", false).TextInfo.ToTitleCase(str);
    }
}