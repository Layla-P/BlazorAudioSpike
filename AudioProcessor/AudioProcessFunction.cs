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
using AudioProcessor.Data;

namespace AudioProcessor
{
    public class AudioProcessFunction
    {

		private readonly IDataRepository _dataRepository;

		public AudioProcessFunction(IDataRepository dataRepository)
		{
			_dataRepository = dataRepository;
		}


		[FunctionName("AudioProcess")]
        public async Task<ActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
			UploadResponse response;
			using (MemoryStream stream = new MemoryStream())
			{
				await req.Body.CopyToAsync(stream);
				byte[] soundBytes = stream.ToArray();
				log.LogInformation(soundBytes.Length.ToString());
				
				//string fileFullPath = Path.Combine(context.FunctionAppDirectory, "temp.mp3");
				
				 //await File.WriteAllBytesAsync(fileFullPath, soundBytes);


				response = await _dataRepository.SaveResponse(soundBytes, ProcessStatusEnum.Uploaded);
				log.LogInformation($"status:{response.GeneralStatusEnum}");
			}

			var downloadFunctionUrl = Environment.GetEnvironmentVariable("DomainUrl")
			   + $"/Download?code={Environment.GetEnvironmentVariable("DownloadAppKey")}&id={response.Id}";

			return response.GeneralStatusEnum != GeneralStatusEnum.Ok
				? new BadRequestObjectResult("Something went wrong")
				: (ActionResult)new OkObjectResult(downloadFunctionUrl);
		}
    }
}
