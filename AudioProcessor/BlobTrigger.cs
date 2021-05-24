using System;
using System.IO;
using System.Threading.Tasks;
using AudioProcessor.Data;
using AudioProcessor.Models;
using AudioProcessor.Services;
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
		private readonly TranscriptionService _transcriptionService;	
		private string transcriptionId = string.Empty;
		private bool isSuccessful = false;
		public BlobTrigger(ILoggerFactory log,
			IOptions<AzStorageConfiguration> blobConfiguration,
			TranscriptionService transcriptionService,
			ITableDbContext tableContext)
		{
			_log = log.CreateLogger<BlobTrigger>();
			_blobConfiguration = blobConfiguration.Value;
			_tableDbContext = tableContext ?? throw new ArgumentNullException(nameof(tableContext));
			_transcriptionService = transcriptionService ?? throw new ArgumentNullException(nameof(transcriptionService));
		}

		[FunctionName("BlobTrigger")]
		public async Task Run([BlobTrigger("audiocontainer/UploadsAudio-{id}.mp3", Connection= "StorageConnectionString")] Stream myBlob, string id, ILogger log)
		{
			log.LogInformation($"C# Blob trigger function Processed blob\n Id:{id} \n Size: {myBlob.Length} Bytes");
			var filename = $"UploadsAudio-{id}.mp3";

			var audioUrl = $"https://audioclipstorage.blob.core.windows.net/{_blobConfiguration.BlobContainerName}/{filename}";


			log.LogInformation(audioUrl);

			TranscriptionResponse transcriptionResponse = await UploadAudioSample(audioUrl, filename);
			

			ProcessStatusEnum status = ProcessStatusEnum.Default;

			if (isSuccessful)
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

		private async Task<TranscriptionResponse> UploadAudioSample(string url, string filename)
		{
			var transcriptionRequest = new TranscriptionRequest
			{
				DisplayName = filename, 
				ContentUrls = new[] { url }
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
