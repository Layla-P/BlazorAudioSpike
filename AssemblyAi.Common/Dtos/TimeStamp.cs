using System.Text.Json.Serialization;

namespace AssemblyAi.Common.Dtos
{
    public class TimeStamp
    {
        [JsonPropertyName("start")]
        public int Start { get; set; }
        
        [JsonPropertyName("end")]
        public int End { get; set; }
    }
}