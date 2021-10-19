using RoadStatus.Data.Configuration;

namespace RoadStatus.Console.Configuration
{
        internal record TfLClientConfiguration : ITfLClientConfiguration
        {
            public static string Section => "TfLApiConfig";
            public string BaseUrl { get; set; }
            public string AppKey { get; set; }
        }
}