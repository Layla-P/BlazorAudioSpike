
using AudioProcessor.Models;
using System.Threading.Tasks;

namespace AudioProcessor.Data
{
	public interface IBlobContext
	{
		Task<(GeneralStatusEnum status, string url)> Write(byte[] imageBytes, string fileName);
	}
}