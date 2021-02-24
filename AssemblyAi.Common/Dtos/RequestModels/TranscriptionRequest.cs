using System.Collections.Generic;
using System.Text.Json.Serialization;
using AssemblyAi.Common.Enums;
using AssemblyAi.Common.Helpers;

namespace AssemblyAi.Common.Dtos.RequestModels
{
    public class TranscriptionRequest : BaseTranscriptionRequest
    {
        [JsonPropertyName("word_boost")] public IEnumerable<string> WordBoost { get; set; } = new List<string>();

         [JsonPropertyName("boost_param")] 
         [JsonConverter(typeof(EnumConvertor<BoostParamEnum>))]
         public BoostParamEnum BoostParam { get; set; } = BoostParamEnum.Default;

         //[JsonPropertyName("webhook_url")] public string WebhookUrl { get; set; } = string.Empty;

         [JsonPropertyName("dual_channel")] public bool DualChannel { get; set; } = false;
        
         [JsonPropertyName("punctuate")] public bool Punctuate { get; set; } = true;
        
         [JsonPropertyName("format_text")] public bool FormatText { get; set; } = true;

        [JsonPropertyName("speaker_labels")] public bool SpeakerLabels { get; set; } = false;

        [JsonPropertyName("acoustic_model")]
        [JsonConverter(typeof(EnumConvertor<AcousticModelEnum>))]
        public AcousticModelEnum AcousticModelEnum { get; set; } = AcousticModelEnum.Default;

        [JsonPropertyName("auto_highlights")] public bool AutoHighlights { get; set; } = false;
        
    }
}