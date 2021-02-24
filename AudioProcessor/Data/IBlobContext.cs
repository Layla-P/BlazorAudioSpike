
using AudioProcessor.Models;
using System.Threading.Tasks;

namespace AudioProcessor.Data
{
	public interface IBlobContext
	{
		Task<GeneralStatusEnum> Write(byte[] imageBytes, string fileName);
	}
}