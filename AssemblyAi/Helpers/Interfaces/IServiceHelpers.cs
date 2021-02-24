using System.Net.Http;
using System.Threading.Tasks;
using AssemblyAi.Common.Dtos;
using AssemblyAi.Common.Dtos.RequestModels;
using AssemblyAi.Common.Helpers;

namespace AssemblyAi.Helpers.Interfaces
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