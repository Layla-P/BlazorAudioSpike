using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AssemblyAi.Common.Dtos.RequestModels
{
    public class RedactionTranscriptionRequest : TranscriptionRequest
    {
        
        [JsonPropertyName("redact_pii")] public bool RedactPii { get; set; } = false;

        [JsonPropertyName("redact_pii_audio")] public bool RedactPiiAudio { get; set; } = false;

        [JsonPropertyName("redact_pii_sub")] public string RedactPiiSub { get; set; }

        [JsonPropertyName("redact_pii_policies")]
        public IEnumerable<string> RedactPiiPolicies { get; set; }
    }
}