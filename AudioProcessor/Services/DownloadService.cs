using Microsoft.Extensions.Logging;
using AudioProcessor.Data;
using AudioProcessor.Models;
using System.Threading.Tasks;

namespace AudioProcessor.Services
{
    public class DownloadService : IDownloadService
    {
        private readonly ITableDbContext _tableDbContext;
        private ILogger _log;

        public DownloadService(ILoggerFactory log, ITableDbContext tableDbContext)
        {
            _log = log.CreateLogger<DownloadService>();
            _tableDbContext = tableDbContext;
        }

        public async Task<DownloadResponse> FetchDownload(string id)
        {
            var downloadResponse = new DownloadResponse();

            var audioEntity = await _tableDbContext.GetEntityAsync("Uploads", id);

            // use try/parse instead?
            if (audioEntity.ProcessStatusEnum != ProcessStatusEnum.Completed.EnumValue())
            {
                _log.LogInformation(audioEntity.ProcessStatusEnum);
                downloadResponse.GeneralStatusEnum = GeneralStatusEnum.Processing;
                downloadResponse.AudioEntity = null;
            }
            else
            {
                downloadResponse.GeneralStatusEnum = GeneralStatusEnum.Ok;
                downloadResponse.AudioEntity = audioEntity;
            }

            return downloadResponse;
        }
    }
}
