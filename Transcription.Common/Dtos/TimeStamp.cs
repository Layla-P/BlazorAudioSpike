using System.Text.Json.Serialization;

namespace Transcription.Common.Dtos
{
    public class TimeStamp
    {
        [JsonPropertyName("start")]
        public int Start { get; set; }
        
        [JsonPropertyName("end")]
        public int End { get; set; }
    }
}