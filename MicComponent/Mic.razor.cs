using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicComponent
{
	public partial class Mic
	{
		public ElementReference MicElement { get; set; }
		[Inject] IJSRuntime JsRuntime { get; set; }
		Lazy<Task<IJSObjectReference>> scriptLoaderModuleTask;
		Lazy<Task<IJSObjectReference>> moduleTask;
		const string ScriptLoaderModulePath = "./_content/MicComponent/scriptloader.js";
		const string JsModulePath = "./_content/MicComponent/mic.js";

		const string audioRecorderJsPath = "./_content/MicComponent/webAudioRecorderJs/WebAudioRecorder.js";
		const string audioRecorderDir = "./_content/MicComponent/webAudioRecorderJs/";
		string errorMessage = null;
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{

			if (firstRender)
			{


				scriptLoaderModuleTask = new(() => JsRuntime.InvokeAsync<IJSObjectReference>("import", ScriptLoaderModulePath)
												.AsTask());
				var scriptLoaderModule = await scriptLoaderModuleTask.Value;
				await scriptLoaderModule.InvokeVoidAsync("loadScript", audioRecorderJsPath);
				await scriptLoaderModule.InvokeVoidAsync("mp3Lame");
				moduleTask = new(() => JsRuntime.InvokeAsync<IJSObjectReference>("import", JsModulePath)
												.AsTask());
				var micModule = await moduleTask.Value;
				await micModule.InvokeVoidAsync("initialize", MicElement, DotNetObjectReference.Create(this));
			}
		}

		async Task Record()
		{
			var micModule = await moduleTask.Value;
			await micModule.InvokeAsync<string>("record", audioRecorderDir, DotNetObjectReference.Create(this));
		}

		
	}
}


//https://stackoverflow.com/questions/44070437/how-to-get-a-file-or-blob-from-an-url-in-javascript