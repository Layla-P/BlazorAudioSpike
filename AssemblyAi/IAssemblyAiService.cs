using System.Threading.Tasks;
using AssemblyAi.Common.Dtos;
using AssemblyAi.Common.Dtos.RequestModels;

namespace AssemblyAi
{
	public interface IAssemblyAiService
	{
		Task<TranscriptionResponse> RetrieveAudioFileAsync(string transcriptId);
		Task<TranscriptionResponse> SubmitAudioFileAsync(TranscriptionRequest transcriptionRequest);
	}
}