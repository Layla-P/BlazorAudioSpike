using System.Text.Json.Serialization;

namespace Transcription.Common.Dtos
{
    public class HighlightResult
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("rank")]
        public decimal Rank { get; set; }

        [JsonPropertyName("timestamps")]
        public TimeStamp Timestamps { get; set; }
       
    }
}