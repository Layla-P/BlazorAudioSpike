using System.Threading.Tasks;
using Transcription.Common.Dtos;
using Transcription.Common.Dtos.RequestModels;

namespace Transcription
{
	public interface ITranscriptionService
	{
		Task<TranscriptionResponse> RetrieveAudioFileAsync(string transcriptId);
		Task<TranscriptionResponse> SubmitAudioFileAsync(TranscriptionRequest transcriptionRequest);
	}
}