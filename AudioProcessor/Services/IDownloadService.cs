using AudioProcessor.Models;
using System.Threading.Tasks;

namespace AudioProcessor.Services
{
    public interface IDownloadService
    {
        Task<DownloadResponse> FetchDownload(string id);
    }
}