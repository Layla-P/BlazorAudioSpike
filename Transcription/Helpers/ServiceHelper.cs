using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using Transcription.Common.Dtos;
using Transcription.Common.Dtos.RequestModels;
using Transcription.Common.Helpers;
using Transcription.Helpers.Interfaces;
using Microsoft.Extensions.Options;

namespace Transcription.Helpers
{
	public class ServiceHelper : IServiceHelpers
	{
		private readonly HttpClient _httpClient;
		private readonly TranscriptionAccount _transcriptionAccount;
		
		public ServiceHelper(HttpClient httpClient, IOptions<TranscriptionAccount> transcriptionAccount)
		{
			_transcriptionAccount = transcriptionAccount.Value ?? throw new ArgumentNullException(nameof(transcriptionAccount));
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_httpClient.DefaultRequestHeaders.Add("Authorization", _transcriptionAccount.AuthToken);
		}

		public async Task<TranscriptionResponse> SubmitAsync(TranscriptionRequest transcriptionRequest)
		{
			StringContent payload = ConvertToStringContent(transcriptionRequest);
			return await PostAsync(payload);
		}

		public async Task<ServiceResponse<TranscriptionResponse>> RetrieveAsync(string transcriptionId)
		{
			_httpClient.DefaultRequestHeaders.Add("Accepts", "application/json");
			HttpResponseMessage response = await _httpClient.GetAsync($"{_transcriptionAccount.Endpoint}/{transcriptionId}");

			var responseJson = await response.Content.ReadAsStringAsync();
			
			var result = new ServiceResponse<TranscriptionResponse>
			{
				HttpStatusCode = response.StatusCode,
				Message =  "Successful",
				ErrorMessage = string.Empty,
				Content = ConvertToTranscriptionResponse(responseJson)
			};

			return result;
		}

		public StringContent ConvertToStringContent(TranscriptionRequest transcriptionRequest)
		{
			string jsonString = JsonSerializer.Serialize(transcriptionRequest);

			return new StringContent(jsonString, Encoding.UTF8, "application/json");
		}
		public TranscriptionResponse ConvertToTranscriptionResponse(string transcriptionResponse)
		{
			return JsonSerializer.Deserialize<TranscriptionResponse>(transcriptionResponse);
		}

		public async Task<TranscriptionResponse> PostAsync(StringContent payload)
		{
			HttpResponseMessage response = await _httpClient.PostAsync(_transcriptionAccount.Endpoint, payload);
			//todo add better error handling in here
			response.EnsureSuccessStatusCode();
			string responseJson = await response.Content.ReadAsStringAsync();
			return ConvertToTranscriptionResponse(responseJson);
		}
	}
}
