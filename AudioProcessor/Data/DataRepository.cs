using System;
using System.Threading.Tasks;
using Transcription;
using Transcription.Common.Dtos.RequestModels;
using AudioProcessor.Models;
using Microsoft.Extensions.Logging;

namespace AudioProcessor.Data
{
	public class DataRepository : IDataRepository
	{
		private readonly IBlobContext _blobContext;
		private readonly ITableDbContext _tableDbContext;
		private readonly ITranscriptionService _transcriptionService;
		private readonly ILogger _log;
		private Guid id;
		private string transcriptionId = string.Empty;
		public DataRepository(
			IBlobContext blobContext,
			ITableDbContext tableDbContext,
			ITranscriptionService transcriptionService,
			ILoggerFactory log)
		{
			_blobContext = blobContext;
			_tableDbContext = tableDbContext;
			_transcriptionService = transcriptionService ?? throw new ArgumentNullException(nameof(transcriptionService));
			_log = log.CreateLogger<DataRepository>();
		}

		public async Task<UploadResponse> SaveResponse(byte[] audioBytes, ProcessStatusEnum status)
		{
			var uploadResponse = new UploadResponse();
			uploadResponse.GeneralStatusEnum = await Save(audioBytes, status);
			uploadResponse.Id = transcriptionId;
			return uploadResponse;
		}

		public async Task<GeneralStatusEnum> Save(byte[] audioBytes, ProcessStatusEnum status)
		{
			SetId();
			var fileName = $"UploadsAudio-{id}.mp3";

			_log.LogInformation($"audioId: {id}");

			var blobResponse = await _blobContext.Write(audioBytes, fileName);

			if (blobResponse.status == GeneralStatusEnum.BadRequest)
			{
				return blobResponse.status;
			}

			return blobResponse.status;
		}

		

		public async Task<GeneralStatusEnum> UpdateTable(string id, ProcessStatusEnum status, string audioUrl = null)
		{
			var fileName = $"UploadsImage-{id}.jpg";
			var generalStatusCode = await SaveAudioDetails(fileName, id, status, audioUrl);
			return generalStatusCode;
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

		//public async Task<bool> CheckTableRecordAvailable(string id)
		//{

		//	var record = await _tableDbContext.GetEntityAsync("Uploads", id);

		//	return record == null ? false : true;

		//}

		private void SetId()
		{
			id = Guid.NewGuid();
		}
	}
}