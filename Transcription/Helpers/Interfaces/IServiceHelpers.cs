using System.Net.Http;
using System.Threading.Tasks;
using Transcription.Common.Dtos;
using Transcription.Common.Dtos.RequestModels;
using Transcription.Common.Helpers;

namespace Transcription.Helpers.Interfaces
{

	public interface IServiceHelpers
	{
		StringContent ConvertToStringContent(TranscriptionRequest transcriptionRequest);
		TranscriptionResponse ConvertToTranscriptionResponse(string transcriptionResponse);
		Task<TranscriptionResponse> PostAsync(StringContent payload);
		Task<TranscriptionResponse> SubmitAsync(TranscriptionRequest transcriptionRequest);
		Task<ServiceResponse<TranscriptionResponse>> RetrieveAsync(string transcriptionId);
	}
}