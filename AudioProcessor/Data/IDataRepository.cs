
using AudioProcessor.Models;
using System.Threading.Tasks;

namespace AudioProcessor.Data
{
	public interface IDataRepository
	{
		Task<UploadResponse> SaveResponse(byte[] imageBytes, ProcessStatusEnum status);

		Task<GeneralStatusEnum> Save(byte[] imageBytes, ProcessStatusEnum status);

		Task<GeneralStatusEnum> UpdateTable(string id, ProcessStatusEnum status, string imageUrl = null);

		//Task<bool> CheckTableRecordAvailable(string id);
	}
}