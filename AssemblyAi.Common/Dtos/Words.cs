using System.Text.Json.Serialization;

namespace AssemblyAi.Common.Dtos
{
	public class Words
	{
		[JsonPropertyName("confidence")]
		public double Confidence { get; set; }

		[JsonPropertyName("end")]
		public int End { get; set; }

		[JsonPropertyName("start")]
		public int Start { get; set; }

		[JsonPropertyName("text")]
		public string Text { get; set; }
		
		[JsonPropertyName("speaker")]
		public string Speaker { get; set; }
	}
}
