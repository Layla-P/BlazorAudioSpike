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
			[HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
			ILogger log)
		{
			log.LogInformation("C# HTTP trigger function processed a request.");

			string id = req.Query["id"];

			if (String.IsNullOrEmpty(id))
			{
				return new BadRequestObjectResult("Please pass a id on the query string");
			}


			if (_downloadService == null)
			{
				log.LogError("download service null");
				return new BadRequestObjectResult("error");
			}

			DownloadResponse downloadResponse;
			try
			{
				downloadResponse = await _downloadService.FetchDownload(id);
			}
			catch (Exception e)
			{
				log.LogError($"Error from download service:{e}");
				return new BadRequestObjectResult(e);
			}

			return downloadResponse.GeneralStatusEnum == GeneralStatusEnum.Ok
				? new JsonResult(downloadResponse)
				: new StatusCodeResult(102) as IActionResult;
		}
	}
}