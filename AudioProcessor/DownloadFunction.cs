using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AudioProcessor.Models;
using AudioProcessor.Services;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;

namespace AudioProcessor
{
	public class DownloadFunction
	{
		private readonly IDownloadService _downloadService;

		public DownloadFunction(IDownloadService downloadService)
		{
			_downloadService = downloadService;
		}

		[FunctionName("Download")]
		//[Route("/api/download/{id}")]
		public async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "post")] WebhookResponse webhookResponse,
			ILogger log)
		{
			log.LogInformation("C# HTTP trigger function processed a request.");



			if (_downloadService == null)
			{
				log.LogError("download service null");
				return new BadRequestObjectResult("error");
			}
			if(webhookResponse.TranscriptId is null ||webhookResponse.Status!= "completed")
			{
				return new BadRequestObjectResult("errors galore!!");
			}

			DownloadResponse downloadResponse;
			try
			{
				downloadResponse = await _downloadService.FetchDownload(webhookResponse.TranscriptId);
				log.LogWarning(downloadResponse.TranscriptionResponse.Text);
			}
			catch (Exception e)
			{
				log.LogError($"Error from download service:{e}");
				return new BadRequestObjectResult(e);
			}

			return  new JsonResult(downloadResponse);
		}
	}
}