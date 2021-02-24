using System;
using System.Threading.Tasks;
using AudioProcessor.Models;
using Microsoft.Extensions.Logging;

namespace AudioProcessor.Data
{
	public class DataRepository : IDataRepository
	{
		private readonly IBlobContext _blobContext;
		//private readonly ITableDbContext _tableDbContext;
		private readonly ILogger _log;
		private Guid id;
		public DataRepository(
			IBlobContext blobContext,
			//ITableDbContext tableDbContext,
			ILoggerFactory log)
		{
			_blobContext = blobContext;
			//_tableDbContext = tableDbContext;
			_log = log.CreateLogger<DataRepository>();
		}

		public async Task<UploadResponse> SaveResponse(byte[] imageBytes, ProcessStatusEnum status)
		{
			var uploadResponse = new UploadResponse();
			uploadResponse.GeneralStatusEnum = await Save(imageBytes, status);
			uploadResponse.Id = id.ToString();
			return uploadResponse;
		}

		public async Task<GeneralStatusEnum> Save(byte[] audioBytes, ProcessStatusEnum status)
		{
			SetId();
			var fileName = $"UploadsAudio-{id}.mp3";

			_log.LogInformation($"audioId: {id}");

			var generalStatusCode = await _blobContext.Write(audioBytes, fileName);

			if (generalStatusCode == GeneralStatusEnum.BadRequest)
			{
				return generalStatusCode;
			}

			//generalStatusCode = await SaveAudioDetails(fileName, id.ToString(), status);

			return generalStatusCode;
		}

		//public async Task<GeneralStatusEnum> UpdateTable(string id, ProcessStatusEnum status, string audioUrl = null)
		//{
		//	var fileName = $"UploadsImage-{id}.jpg";
		//	var generalStatusCode = await SaveAudioDetails(fileName, id, status, audioUrl);
		//	return generalStatusCode;
		//}

		//private async Task<GeneralStatusEnum> SaveAudioDetails(
		//	string fileName,
		//	string id,
		//	ProcessStatusEnum status,
		//	string audioUrl = null)
		//{
		//	var uploadEntity = new AudioEntity("Uploads", id, fileName, status, audioUrl);
		//	return await _tableDbContext.InsertOrMergeEntityAsync(uploadEntity);
		//}

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