using Microsoft.Extensions.Configuration;
using RoadStatus.Data.Configuration;

namespace RoadStatus.UnitTests.Configuration
{
    internal record TestConfiguration : ITfLClientConfiguration
    {
        public const string Section = "TfLApiConfig";

        public string BaseUrl { get; set; }
        public string AppKey { get; set; }

        public TestConfiguration() =>
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build()
                .GetSection(Section).
                Bind(this);
    }
}
