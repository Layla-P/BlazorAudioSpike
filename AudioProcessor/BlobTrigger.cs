using System;
using System.IO;
using System.Threading.Tasks;
using AssemblyAi;
using AssemblyAi.Common.Dtos.RequestModels;
using AudioProcessor.Data;
using AudioProcessor.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AudioProcessor
{
	public class BlobTrigger
	{
		private ILogger _log;
		private readonly AzStorageConfiguration _blobConfiguration;
		private readonly ITableDbContext _tableDbContext;
		private readonly IAssemblyAiService _assemblyAiService;	
		private string transcriptionId = string.Empty;
		public BlobTrigger(ILoggerFactory log,
			IOptions<AzStorageConfiguration> blobConfiguration,
			IAssemblyAiService assemblyAiService,
			ITableDbContext tableContext)
		{
			_log = log.CreateLogger<BlobTrigger>();
			_blobConfiguration = blobConfiguration.Value;
			_tableDbContext = tableContext ?? throw new ArgumentNullException(nameof(tableContext));
			_assemblyAiService = assemblyAiService ?? throw new ArgumentNullException(nameof(assemblyAiService));
		}

		[FunctionName("BlobTrigger")]
		public async Task Run([BlobTrigger("audiocontainer/UploadsAudio-{id}.mp3", Connection= "StorageConnectionString")] Stream myBlob, string id, ILogger log)
		{
			log.LogInformation($"C# Blob trigger function Processed blob\n Id:{id} \n Size: {myBlob.Length} Bytes");
			var filename = $"UploadsAudio-{id}.mp3";

			var audioUrl = $"https://audioclipstorage.blob.core.windows.net/{_blobConfiguration.BlobContainerName}/{filename}";


			log.LogInformation(audioUrl);

			TranscriptionResponse assemblyresponse = await UploadAudioSample(audioUrl);
			transcriptionId = assemblyresponse.Id;

			ProcessStatusEnum status = ProcessStatusEnum.Default;

			if (assemblyresponse.Status == "queued")
			{
				status = ProcessStatusEnum.Processing;
			}
			else
			{
				status = ProcessStatusEnum.Failed;
			}

			GeneralStatusEnum statuscode = await SaveAudioDetails(filename, transcriptionId, status, audioUrl);

			log.LogInformation($"Process status: {statuscode}, Audio URL: {audioUrl}, TranscriptionId: {transcriptionId}");
		}

		private async Task<TranscriptionResponse> UploadAudioSample(string url)
		{
			var transcriptionRequest = new TranscriptionRequest
			{
				AudioUrl = url,
				WebhookUrl = "https://e07ee3af6977.ngrok.io/api/download"
			};

			return await _assemblyAiService.SubmitAudioFileAsync(transcriptionRequest);
		}


		private async Task<GeneralStatusEnum> SaveAudioDetails(
			string fileName,
			string id,
			ProcessStatusEnum status,
			string audioUrl = null)
		{
			var uploadEntity = new AudioEntity("Uploads", id, fileName, status, audioUrl);
			return await _tableDbContext.InsertOrMergeEntityAsync(uploadEntity);
		}

	}
}
