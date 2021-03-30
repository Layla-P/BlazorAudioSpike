
using AudioProcessor.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;

namespace AudioProcessor.Data
{
	public interface IBlobContext
	{
		Task<(GeneralStatusEnum status, string url)> Write(byte[] imageBytes, string fileName);
		
	}
}