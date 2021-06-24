using Newtonsoft.Json;


namespace AudioProcessor.Models
{
	public class WebhookResponse
	{

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("transcript_id")]
		public string TranscriptId { get; set; }

	}
}
