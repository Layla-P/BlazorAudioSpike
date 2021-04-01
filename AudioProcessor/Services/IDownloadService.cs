using AudioProcessor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AudioProcessor.Services
{
    public interface IDownloadService
    {
        Task<DownloadResponse> FetchDownload(string id);
		Task<IEnumerable<DownloadResponse>> FetchDownloads();

	}
}