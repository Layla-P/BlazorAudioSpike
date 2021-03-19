namespace AudioProcessor.Models
{
	public class AzStorageConfiguration
	{
		public string StorageConnectionString { get; set; }
		public  string BlobContainerName { get; set; }
		public string TableName { get; set; }
	}
}
