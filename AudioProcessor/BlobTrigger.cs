using System;
using System.IO;
using System.Threading.Tasks;
using Transcription;
using Transcription.Common.Dtos.RequestModels;
using AudioProcessor.Data;
using AudioProcessor.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AudioProcessor
{
	public class BlobTrigger
	{
		private ILogger _log;
		private readonly AzStorageConfiguration _blobConfiguration;
		private readonly ITableDbContext _tableDbContext;
		private readonly ITranscriptionService _transcriptionService;	
		private string transcriptionId = string.Empty;
		private const string NGROK_URL_STRING = "d356-86-165-30-241";
		public BlobTrigger(ILoggerFactory log,
			IOptions<AzStorageConfiguration> blobConfiguration,
			ITranscriptionService transcriptionService,
			ITableDbContext tableContext)
		{
			_log = log.CreateLogger<BlobTrigger>();
			_blobConfiguration = blobConfiguration.Value;
			_tableDbContext = tableContext ?? throw new ArgumentNullException(nameof(tableContext));
			_transcriptionService = transcriptionService ?? throw new ArgumentNullException(nameof(transcriptionService));
		}

		[FunctionName("BlobTrigger")]
		public async Task Run([BlobTrigger("audiocontainer/UploadsAudio-{id}.mp3")] Stream myBlob, string id, ILogger log)
		{
			
			log.LogInformation($"C# Blob trigger function Processed blob\n Id:{id} \n Size: {myBlob.Length} Bytes");
			var filename = $"UploadsAudio-{id}.mp3";

			var audioUrl = $"https://audioclipstorage.blob.core.windows.net/{_blobConfiguration.BlobContainerName}/{filename}";


			log.LogInformation(audioUrl);

			TranscriptionResponse transcriptionResponse = await UploadAudioSample(audioUrl);
			transcriptionId = transcriptionResponse.Id;
			log.LogInformation($"Transcription ID: {transcriptionId}");
			ProcessStatusEnum status = ProcessStatusEnum.Default;

			if (transcriptionResponse.Status == "queued")
			{
				status = ProcessStatusEnum.Processing;
			}
			else
			{
				status = ProcessStatusEnum.Failed;
			}

			GeneralStatusEnum statuscode = await SaveAudioDetails(filename, transcriptionId, status, audioUrl);

			log.LogInformation($"Process status: {statuscode}, Audio URL: {audioUrl}");
		}

		private async Task<TranscriptionResponse> UploadAudioSample(string url)
		{
			var transcriptionRequest = new TranscriptionRequest
			{
				AudioUrl = url,
				WebhookUrl = $"https://{NGROK_URL_STRING}.ngrok.io/api/download"
			};

			return await _transcriptionService.SubmitAudioFileAsync(transcriptionRequest);
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
