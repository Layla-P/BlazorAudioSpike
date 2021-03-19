using AudioProcessor.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;

namespace AudioProcessor.Data
{
	public class BlobContext : IBlobContext
	{
		private ILogger _log;
		private readonly AzStorageConfiguration _blobConfiguration;
		public BlobContext(ILoggerFactory log, IOptions<AzStorageConfiguration> blobConfiguration)
		{
			_log = log.CreateLogger<BlobContext>();
			_blobConfiguration = blobConfiguration.Value;
		}

		public async Task<(GeneralStatusEnum status, string url)> Write(byte[] audioBytes, string fileName)
		{

			//var storageConnectionString = Environment.GetEnvironmentVariable("StorageConnectionString");
			_log.LogInformation("storageConnectionString:" + _blobConfiguration.StorageConnectionString);

			if (CloudStorageAccount.TryParse(_blobConfiguration.StorageConnectionString, out CloudStorageAccount storageAccount))
			{
				// Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
				CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();


				//var blobContainerName = Environment.GetEnvironmentVariable("BlobContainerName");
				CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(_blobConfiguration.BlobContainerName);
				await cloudBlobContainer.CreateIfNotExistsAsync();

				CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
				cloudBlockBlob.Properties.ContentType = "audio/mpeg";

				try
				{
					await cloudBlockBlob.UploadFromByteArrayAsync(audioBytes, 0, audioBytes.Length);
				}
				catch (StorageException ex)
				{
					if (ex.RequestInformation.HttpStatusCode == (int)System.Net.HttpStatusCode.Conflict)
						return (GeneralStatusEnum.BadRequest, string.Empty);
				}
			}

			var url = $"https://audioclipstorage.blob.core.windows.net/{_blobConfiguration.BlobContainerName}/{fileName}";

			return (GeneralStatusEnum.Ok, url);
			// 200 = Ok, 202 = resource created (200 + resource location)
		}
	}
}