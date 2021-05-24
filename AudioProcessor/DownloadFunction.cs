using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AudioProcessor.Services;
using System.Text;
using System.Collections.Generic;
using System.Web.Http;
using System.Security.Cryptography;
using System.Linq;
using Microsoft.Extensions.Options;
using AudioProcessor.Models;
using BatchClient;

namespace AudioProcessor
{
	public class DownloadFunction
	{
		private readonly IDownloadService _downloadService;
		private readonly SpeechConfiguration _config;
		private const string webHookSecret = "suopnwfnsfnjdfwelrferjfglsngjerhglkmflkdf";
		private const string WebHookEventKindHeaderName = "X-MicrosoftSpeechServices-Event";
		public const string WebHookSignatureHeaderName = "X-MicrosoftSpeechServices-Signature";
		private const string ValidationTokenKeyQueryParameterName = "validationToken";

		public DownloadFunction(IOptions<SpeechConfiguration> config, IDownloadService downloadService)
		{
			_downloadService = downloadService;
			_config = config.Value;
		}

		[FunctionName("Download")]
		//[Route("/api/download/{id}")]
		public async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest request,
			ILogger log)
		{
			log.LogInformation("Download trigger hit");

			var headers = new Dictionary<string, string>();
			foreach (var header in request.Headers)
			{
				headers.Add(header.Key, header.Value.ToString());
			}

			if (!headers.TryGetValue(WebHookEventKindHeaderName, out var eventKindString) ||
				!Enum.TryParse<BatchClient.WebHookEventKind>(eventKindString, out var eventKind))
			{
				var message = $"Missing or invalid value for required header \"{WebHookEventKindHeaderName}\".";
				log.LogError(message);

				return new BadRequestErrorMessageResult(message);
			}
			string requestBody = null, validationToken = null, payload;
			if (eventKind == WebHookEventKind.Challenge)
			{
				validationToken = request.Query[ValidationTokenKeyQueryParameterName].FirstOrDefault();
				payload = validationToken;
			}
			else
			{
				requestBody = await new StreamReader(request.Body).ReadToEndAsync();
				payload = requestBody;
			}

			if (headers.TryGetValue(WebHookSignatureHeaderName, out var actualSignature))
			{
				var contentBytes = Encoding.UTF8.GetBytes(payload);
				var secretBytes = Encoding.UTF8.GetBytes(webHookSecret);
				using (var hmacsha256 = new HMACSHA256(secretBytes))
				{
					var hash = hmacsha256.ComputeHash(contentBytes);
					var expectedSignature = Convert.ToBase64String(hash);

					if (expectedSignature != actualSignature)
					{
						log.LogError($"Notification has invalid signature.");
						return new BadRequestErrorMessageResult("Invalid signature detected.");
					}
				}
			}

			if (eventKind == BatchClient.WebHookEventKind.Challenge)
			{
				log.LogInformation("Received challenge and responded.");

				return new OkObjectResult(validationToken);
			}

			log.LogInformation($"Received web hook notification, kind={eventKindString}");

			var webHookNotification = JsonConvert.DeserializeObject<WebHookNotification>(requestBody);

			// invocationId can be used for deduplication, it's unique per notification event
			log.LogInformation($"Processing notification {webHookNotification.InvocationId}.");
			RecognitionResults result;
			using (var client = BatchClient.BatchClient.CreateApiV3Client(_config.SubscriptionKey, $"{_config.Region}.api.cognitive.microsoft.com"))
			{
				if (eventKind == WebHookEventKind.TranscriptionCompletion)
				{
					var transcription = await client.GetTranscriptionAsync(webHookNotification.Self).ConfigureAwait(false);
					var paginatedfiles = await client.GetTranscriptionFilesAsync(transcription.Links.Files).ConfigureAwait(false);
					var resultFile = paginatedfiles.Values.FirstOrDefault(f => f.Kind == ArtifactKind.Transcription);
					result = await client.GetTranscriptionResultAsync(new Uri(resultFile.Links.ContentUrl)).ConfigureAwait(false);

					log.LogInformation("Transcription succeeded. Results: ");
					log.LogInformation(JsonConvert.SerializeObject(result, SpeechJsonContractResolver.WriterSettings));

					await client.DeleteTranscriptionAsync(webHookNotification.Self).ConfigureAwait(false);
				}
			}

			if (_downloadService == null)
			{
				log.LogError("download service null");
				return new BadRequestObjectResult("error");
			}
			

			//DownloadResponse downloadResponse;
			//try
			//{
				

			//}
			//catch (Exception e)
			//{
			//	log.LogError($"Error from download service:{e}");
			//	return new BadRequestObjectResult(e);
			//}

			return new OkResult();
		}


	}
}