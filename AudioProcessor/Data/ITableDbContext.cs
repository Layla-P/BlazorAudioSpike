using AudioProcessor.Models;
using System.Threading.Tasks;

namespace AudioProcessor.Data
{
	public interface ITableDbContext
	{
		Task CreateTableAsync();
		Task<GeneralStatusEnum> InsertOrMergeEntityAsync(AudioEntity entity);

		Task<AudioEntity> GetEntityAsync(string partitionKey, string rowKey);

	}

}