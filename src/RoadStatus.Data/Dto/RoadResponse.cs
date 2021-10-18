using Newtonsoft.Json;

namespace RoadStatus.Data.Dto
{
    internal record RoadResponse
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("displayName")] public string DisplayName { get; set; }

        [JsonProperty("statusSeverity")] public string StatusSeverity { get; set; }

        [JsonProperty("statusSeverityDescription")]
        public string StatusSeverityDescription { get; set; }
    }
}