using Newtonsoft.Json;

namespace RoadStatus.EndToEndTests.Records
{
    internal record RoadResponse
    {
        [JsonProperty("$type")] public string Type { get; set; }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("displayName")] public string DisplayName { get; set; }

        [JsonProperty("statusSeverity")] public string StatusSeverity { get; set; }

        [JsonProperty("statusSeverityDescription")]
        public string StatusSeverityDescription { get; set; }

        [JsonProperty("bounds")] public string Bounds { get; set; }

        [JsonProperty("envelope")] public string Envelope { get; set; }

        [JsonProperty("url")] public string Url { get; set; }
    }
}