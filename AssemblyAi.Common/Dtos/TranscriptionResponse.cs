using System.Collections.Generic;
using System.Text.Json.Serialization;
using AssemblyAi.Common.Enums;
using AssemblyAi.Common.Helpers;

namespace AssemblyAi.Common.Dtos
{
    public class TranscriptionResponse
    {
        [JsonPropertyName("id")] public string Id { get; set; }

        [JsonPropertyName("status")] public string Status { get; set; }

        [JsonPropertyName("acoustic_model")]
        [JsonConverter(typeof(EnumConvertor<AcousticModelEnum>))]
        public AcousticModelEnum AcousticModel { get; set; }

        [JsonPropertyName("audio_duration")] public double? AudioDuration { get; set; }

        [JsonPropertyName("audio_url")] public string AudioUrl { get; set; }

        [JsonPropertyName("confidence")] public double? Confidence { get; set; }

        [JsonPropertyName("dual_channel")] public bool? DualChannel { get; set; }

        [JsonPropertyName("format_text")] public bool FormatText { get; set; }

        [JsonPropertyName("language_model")]
        [JsonConverter(typeof(EnumConvertor<AcousticModelEnum>))]
        public AcousticModelEnum LanguageModel { get; set; }

        [JsonPropertyName("punctuate")] public bool Punctuate { get; set; }

        [JsonPropertyName("text")] public string Text { get; set; }

        [JsonPropertyName("utterances")] public IEnumerable<Utterance> Utterances { get; set; }

        [JsonPropertyName("webhook_status_code")]
        public string WebhookStatusCode { get; set; }

        [JsonPropertyName("webhook_url")] public string WebhookUrl { get; set; }

        [JsonPropertyName("words")] public IEnumerable<Words> Words { get; set; }

        [JsonPropertyName("auto_highlights_result")]
        public AutoHighlightsResult AutoHighlightsResult { get; set; }
    }
}