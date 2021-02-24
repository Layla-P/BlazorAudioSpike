using System.Text.Json.Serialization;

namespace AssemblyAi.Common.Dtos
{
    public abstract class BaseTranscriptionRequest
    {
        [JsonPropertyName("audio_url")] public string AudioUrl { get; set; }
    }
}