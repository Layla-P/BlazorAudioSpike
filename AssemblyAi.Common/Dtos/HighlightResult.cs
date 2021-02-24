using System.Text.Json.Serialization;

namespace AssemblyAi.Common.Dtos
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