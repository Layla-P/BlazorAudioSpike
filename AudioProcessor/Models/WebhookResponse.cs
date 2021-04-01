using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AudioProcessor.Models
{
	public class WebhookResponse
	{

		[JsonPropertyName("status")]
		public string status { get; set; }

		[JsonPropertyName("transcript_id")]
		public string transcript_id { get; set; }

	}
}
