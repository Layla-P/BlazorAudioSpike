using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AudioProcessor
{
    public static class AudioProcessFunction
    {
        [FunctionName("AudioProcess")]
        public static async Task Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
			using (MemoryStream stream = new MemoryStream())
			{
				await req.Body.CopyToAsync(stream);
				byte[] soundBytes = stream.ToArray();
				log.LogInformation(soundBytes.Length.ToString());
				
				string fileFullPath = Path.Combine(context.FunctionAppDirectory, "test.mp3");
				
				 await File.WriteAllBytesAsync(fileFullPath, soundBytes);
				
				log.LogInformation(fileFullPath);
			}
		}
    }
}
