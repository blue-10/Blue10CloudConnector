using System.Text.Json.Serialization;

namespace WindowsServiceFramework.Models
{
    public class VersionModel
    {
        [JsonPropertyName("version")]
        public string Version { get; set; } = "0.0.0.0";

        [JsonIgnore]
        public string[] VersionArray => Version.Split('.');
    }
}