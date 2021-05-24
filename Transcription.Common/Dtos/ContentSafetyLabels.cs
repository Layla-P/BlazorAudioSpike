using System.Text.Json.Serialization;

namespace Transcription.Common.Dtos
{
	public class ContentSafetyLabels
	{

		[JsonPropertyName("status")]
		public string Status { get; set; }

		[JsonPropertyName("results")]
		public Result[] Results { get; set; }

		[JsonPropertyName("summary")]
		public Summary Summary { get; set; }
	}
	public partial class Result
	{
		[JsonPropertyName("text")]
		public string Text { get; set; }

		[JsonPropertyName("labels")]
		public Label[] Labels { get; set; }

		[JsonPropertyName("timestamp")]
		public TimeStamp Timestamp { get; set; }
	}

	public partial class Label
	{
		[JsonPropertyName("confidence")]
		public double Confidence { get; set; }

		[JsonPropertyName("label")]
		public string LabelLabel { get; set; }
	}

	public partial class Summary
	{
		[JsonPropertyName("accidents")] public double Accidents { get; set; }

		[JsonPropertyName("nsfw")] public double Nsfw { get; set; }

		[JsonPropertyName("alcohol")] public double Alcohol { get; set; }
	}
}
