
using Transcription.Common.Dtos.RequestModels;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AudioSpikeBlazorServer.Shared
{
	public partial class SpeechToText
	{

		public ElementReference TextElement { get; set; }
		public string TextContent = string.Empty;
		[Inject] ILocalStorageService localStorage { get; set; }
		[Inject] IHttpClientFactory clientFactory { get; set; }

		[Inject] JsonSerializerOptions serializeOptions { get; set; }
		//[Inject] IJSRuntime JsRuntime { get; set; }
		//Lazy<Task<IJSObjectReference>> moduleTask;
		//		const string JsModulePath = "./_content/MicComponent/speechToText.js";

		string errorMessage = null;
		

		async Task GetText()
		{
			try
			{
				var transcriptionId = await localStorage.GetItemAsync<string>("transcription-id");
				if (transcriptionId is null) { throw new Exception(); }

				var request = new HttpRequestMessage(HttpMethod.Get,$"download/?id={transcriptionId}");

				var client = clientFactory.CreateClient("AudioProcessor");
				var response = await client.SendAsync(request);

				var responseJson = await response.Content.ReadAsStringAsync();
				
				var transcriptionResponse = JsonSerializer.Deserialize<Rootobject>(responseJson).transcriptionResponse;
			
				 
				TextContent = transcriptionResponse.text;
				StateHasChanged();
			}
			catch(Exception ex)
			{
				var exception = ex;
			}
			
		}
	}
}
