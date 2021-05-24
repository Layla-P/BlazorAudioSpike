using System;
using Microsoft.Extensions.Logging;
using AudioProcessor.Data;
using AudioProcessor.Models;
using System.Threading.Tasks;
using Transcription;
using Transcription.Common.Dtos.RequestModels;
using System.Collections.Generic;

namespace AudioProcessor.Services
{
    public class DownloadService : IDownloadService
    {
        private readonly ITableDbContext _tableDbContext;
		private readonly IDataRepository _dataRepository;
		private readonly ITranscriptionService _transcriptionService;
        private ILogger _log;

        public DownloadService(ILoggerFactory log,
			ITableDbContext tableDbContext,
			ITranscriptionService transcriptionService,
			IDataRepository dataRepository)
        {
            _log = log.CreateLogger<DownloadService>();
            _tableDbContext = tableDbContext;
			_transcriptionService = transcriptionService ?? throw new ArgumentNullException(nameof(transcriptionService));
			_dataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));
		}

        public async Task<DownloadResponse> FetchDownload(string id)
        {
            var downloadResponse = new DownloadResponse();

            var audioEntity = await _tableDbContext.GetEntityAsync("Uploads", id);

			// use try/parse instead?
			if (audioEntity.ProcessStatusEnum == ProcessStatusEnum.Failed.EnumValue())
            {
				downloadResponse.GeneralStatusEnum = GeneralStatusEnum.Fail;
				return downloadResponse;
			}
			//signalr to update status.
			var assemblyResponse = await GetTranscription(id);

			if(assemblyResponse.Status == "completed")
			{
				_ = await _dataRepository.UpdateTable(id, ProcessStatusEnum.Completed, null);
				downloadResponse.GeneralStatusEnum = GeneralStatusEnum.Ok;
				downloadResponse.AudioEntity = await _tableDbContext.GetEntityAsync("Uploads", id);
				downloadResponse.TranscriptionResponse = assemblyResponse;
				return downloadResponse;
			}


            return downloadResponse;
        }

		public async Task<IEnumerable<DownloadResponse>> FetchDownloads()
		{
			IEnumerable<DownloadResponse> downloadResponse = new List<DownloadResponse>();

			//todo

			return downloadResponse;
		}
		private async Task<TranscriptionResponse> GetTranscription(string id)
		{
			return await _transcriptionService.RetrieveAudioFileAsync(id);

		}
    }
}
