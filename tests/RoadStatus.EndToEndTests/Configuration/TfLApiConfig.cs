namespace RoadStatus.EndToEndTests.Configuration
{
    public class TfLApiConfig
    {
        public const string Section = "TfL.ApiConfig";

        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
    }
}