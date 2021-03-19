using AssemblyAi.Common.Dtos.RequestModels;

namespace AudioProcessor.Models
{
	public class DownloadResponse : ResponseBase
	{
		public AudioEntity AudioEntity { get; set; }
		public TranscriptionResponse TranscriptionResponse { get; set; }
	}
}