namespace RoadStatus.EndToEndTests.Configuration
{
    public record TfLApiConfig
    {
        public const string Section = "TfL.ApiConfig";

        public string BaseUrl { get; set; }
        public string AppKey { get; set; }
    }
}