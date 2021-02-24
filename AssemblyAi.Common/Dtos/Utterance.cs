using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AssemblyAi.Common.Dtos
{
    public class Utterance
    {
        [JsonPropertyName("speaker")] public string Speaker { get; set; }

        [JsonPropertyName("confidence")] public float Confidence { get; set; }

        [JsonPropertyName("end")] public int End { get; set; }

        [JsonPropertyName("start")] public int Start { get; set; }

        [JsonPropertyName("text")] public string Text { get; set; }

        [JsonPropertyName("words")] public IEnumerable<Words> Words { get; set; }
    }
}