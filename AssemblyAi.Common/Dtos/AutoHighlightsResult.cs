using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AssemblyAi.Common.Dtos
{
    public class AutoHighlightsResult
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }
        
        [JsonPropertyName("results")]
        public IEnumerable<HighlightResult> Results { get; set; }
    }
}