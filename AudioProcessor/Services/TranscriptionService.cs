using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using AudioProcessor.Models;
using System.Text;
using System.Text.Json;
using System.Web;
using System.Net.Http.Headers;

namespace AudioProcessor.Services
{

	public class TranscriptionService
	{
		private readonly HttpClient _httpClient;
		private readonly SpeechConfiguration _config;
		private const string Uri = "https://laylaspeechtotext.cognitiveservices.azure.com/";


		public TranscriptionService(IOptions<SpeechConfiguration> speechConfiguration, HttpClient httpClient)
		{
			_config = speechConfiguration.Value ?? throw new ArgumentNullException(nameof(speechConfiguration));

			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

			SetupWebhook();

		}
		public async Task<TranscriptionResponse> SubmitAudioFileAsync(TranscriptionRequest transcriptionRequest)
		{
			var requestBody = ConvertToStringContent(transcriptionRequest);

			var uri = $"https://{_config.Region}.api.cognitive.microsoft.com/speechtotext/v3.0/transcriptions?";
			//HttpResponseMessage response = await _httpClient.PostAsync($"{Uri}/speechtotext/v3.0/transcriptions", requestBody);
			_httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _config.SubscriptionKey);
			HttpResponseMessage response = await _httpClient.PostAsync(uri, requestBody);
			if (!response.IsSuccessStatusCode)
			{
				string errorResponse = await response.Content.ReadAsStringAsync();
				var test = JsonSerializer.Deserialize<ErrorResponse>(errorResponse);
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

		private async Task SetupWebhook()
		{
			var json = @"{
						  ""displayName"": ""TranscriptionCompletionWebHook"",
						  ""properties"": {
										""secret"": """"
						  },
						  ""webUrl"": ""https://97dc06ee4631.ngrok.io/api/download"",
						  ""events"": {
										""transcriptionCompletion"": true
						  },
						  ""description"": ""I registered this URL to get a POST request for each completed transcription.""
						}";
			var uri = "https://uksouth.api.cognitive.microsoft.com/speechtotext/v3.0/webhooks";
			_httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _config.SubscriptionKey);
			HttpResponseMessage response = await _httpClient.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));
			
				Console.WriteLine(
					response.StatusCode == System.Net.HttpStatusCode.Created ?
					"Webhook created"
					: "Webhook failed");
			

		}
	}
}

