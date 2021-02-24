
namespace AudioProcessor.Models
{
	public interface ITableConfiguration
	{
		string ConnectionString { get; set; }
		string TableName { get; set; }
	}
}