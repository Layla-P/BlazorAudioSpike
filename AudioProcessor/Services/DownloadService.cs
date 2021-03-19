using System;
using Microsoft.Extensions.Logging;
using AudioProcessor.Data;
using AudioProcessor.Models;
using System.Threading.Tasks;
using AssemblyAi;
using AssemblyAi.Common.Dtos.RequestModels;

namespace AudioProcessor.Services
{
    public class DownloadService : IDownloadService
    {
        private readonly ITableDbContext _tableDbContext;
		private readonly IDataRepository _dataRepository;
		private readonly IAssemblyAiService _assemblyAiService;
        private ILogger _log;

        public DownloadService(ILoggerFactory log,
			ITableDbContext tableDbContext,
			IAssemblyAiService assemblyAiService,
			IDataRepository dataRepository)
        {
            _log = log.CreateLogger<DownloadService>();
            _tableDbContext = tableDbContext;
			_assemblyAiService = assemblyAiService ?? throw new ArgumentNullException(nameof(assemblyAiService));
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

		private async Task<TranscriptionResponse> GetTranscription(string id)
		{
			return await _assemblyAiService.RetrieveAudioFileAsync(id);

		}
    }
}
