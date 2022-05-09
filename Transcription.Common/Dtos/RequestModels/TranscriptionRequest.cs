using System.Collections.Generic;
using System.Text.Json.Serialization;
using Transcription.Common.Helpers;

namespace Transcription.Common.Dtos.RequestModels
{
    public class TranscriptionRequest : BaseTranscriptionRequest
    {
        [JsonPropertyName("webhook_url")] public string WebhookUrl { get; set; } = string.Empty;
    }
}