using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using AssemblyAi.Common.Dtos;
using AssemblyAi.Common.Dtos.RequestModels;
using AssemblyAi.Helpers;
using AssemblyAi.Helpers.Interfaces;

namespace AssemblyAi
{
	public class AssemblyAiService : IAssemblyAiService
	{
		private readonly HttpClient _httpClient;
		private readonly IServiceHelpers _helper;
		private const string Uri = "https://api.assemblyai.com/v2/transcript";

		
		public AssemblyAiService(AssemblyAiAccount assemblyAiAccount, HttpClient httpClient, IServiceHelpers helper)
		{
			var account = assemblyAiAccount ?? throw new ArgumentNullException(nameof(assemblyAiAccount));
			_helper = helper ?? throw new ArgumentNullException(nameof(helper));
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_httpClient.DefaultRequestHeaders.Add("Authorization", account.AuthToken);
		}
		public async Task<TranscriptionResponse> SubmitAudioFileAsync(TranscriptionRequest transcriptionRequest)
		{
			return await _helper.SubmitAsync(transcriptionRequest);
		}
		public async Task<TranscriptionResponse> RetrieveAudioFileAsync(string transcriptId)
		{
			_httpClient.DefaultRequestHeaders.Add("Accepts", "application/json");
			HttpResponseMessage response = await _httpClient.GetAsync($"{Uri}/{transcriptId}");
			
			response.EnsureSuccessStatusCode();

			var responseJson = await response.Content.ReadAsStringAsync();
			return _helper.ConvertToTranscriptionResponse(responseJson);
		}


	}
}
