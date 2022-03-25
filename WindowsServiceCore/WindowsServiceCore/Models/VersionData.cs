using System.Text.Json.Serialization;

namespace WindowsServiceCore.Models
{
    public class VersionData
    {
        [JsonPropertyName("version")]
        public string Version { get; set; } = "0.0.0.0";

        [JsonIgnore]
        public string[] VersionArray => Version.Split('.');
    }
}
