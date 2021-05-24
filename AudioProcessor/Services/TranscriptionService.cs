using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using AudioProcessor.Models;
using Microsoft.CognitiveServices.Speech;
using System.Text;
using System.Text.Json;

namespace AudioProcessor.Services
{

	public class TranscriptionService
	{
		private readonly HttpClient _httpClient;
		private readonly SpeechConfig _speechConfig;
		//private readonly IServiceHelpers _helper;
		private const string Uri = "https://laylaspeechtotext.cognitiveservices.azure.com/";


		public TranscriptionService(IOptions<SpeechConfiguration> speechConfiguration, HttpClient httpClient)
		{
			var account = speechConfiguration.Value ?? throw new ArgumentNullException(nameof(speechConfiguration));
			_speechConfig = SpeechConfig.FromSubscription(account.SubscriptionKey, account.Region);
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
			_httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", account.SubscriptionKey);
		}
		public async Task<TranscriptionResponse> SubmitAudioFileAsync(TranscriptionRequest transcriptionRequest)
		{
			var requestBody = ConvertToStringContent(transcriptionRequest);
			HttpResponseMessage response = await _httpClient.PostAsync($"{Uri}/speechtotext/v3.0/transcriptions", requestBody);
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}
			string responseJson = await response.Content.ReadAsStringAsync();
			return ConvertToTranscriptionResponse(responseJson);
			
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
	}
}

