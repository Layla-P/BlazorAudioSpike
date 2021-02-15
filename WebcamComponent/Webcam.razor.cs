using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace WebcamComponent
{
    public partial class Webcam
    {
        const string JsModulePath = "./_content/WebcamComponent/webcam.js";

        [Inject] IJSRuntime JsRuntime { get; set; }

        Lazy<Task<IJSObjectReference>> moduleTask;

        public ElementReference VideoElement { get; set; }

        string errorMessage = null;

        bool isCameraStreaming = false;
        string CssCameraWrapper => isCameraStreaming ? "camera streaming" : "camera unavailable";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                moduleTask = new(() => JsRuntime.InvokeAsync<IJSObjectReference>("import", JsModulePath)
                                                .AsTask());
                var module = await moduleTask.Value;
                await module.InvokeVoidAsync("initialize", VideoElement, DotNetObjectReference.Create(this));
            }
        }

        [JSInvokable]
        public void OnCameraStreaming()
        {
            isCameraStreaming = true;
            StateHasChanged();
        }

        [JSInvokable]
        public void OnCameraStreamingError(string error)
        {
            isCameraStreaming = false;
            errorMessage = error;
            StateHasChanged();
        }

        public async Task<string> GetSnapshot()
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("getSnapshot", VideoElement);
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }

    }
}
