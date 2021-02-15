using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace MicComponent
{
	public partial class Mic
	{
		public ElementReference MicElement { get; set; }
		[Inject] IJSRuntime JsRuntime { get; set; }

		Lazy<Task<IJSObjectReference>> moduleTask;

		const string JsModulePath = "./_content/MicComponent/mic.js";

		string errorMessage = null;
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{

			if (firstRender)
			{
				//await JsRuntime.InvokeVoidAsync("loadScript", "./_content/WebcamComponent/WebAudioRecorder.js");

				moduleTask = new(() => JsRuntime.InvokeAsync<IJSObjectReference>("import", JsModulePath)
												.AsTask());
				var module = await moduleTask.Value;
				await module.InvokeVoidAsync("initialize", MicElement, DotNetObjectReference.Create(this));
			}
		}

		
	}
}
