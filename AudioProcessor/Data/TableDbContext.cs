using System;
using System.Threading.Tasks;
using AudioProcessor.Models;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AudioProcessor.Data
{
	public class TableDbContext : ITableDbContext
	{
		private readonly AzStorageConfiguration _tableConfiguration;

		private CloudTable _table;

		private readonly ILogger _log;


		public TableDbContext(ILoggerFactory log, IOptions<AzStorageConfiguration> tableConfiguration)
		{
			_log = log.CreateLogger<TableDbContext>();			
			_tableConfiguration = tableConfiguration.Value;

		}


		public async Task CreateTableAsync()
		{
			// Retrieve storage account information from connection string.
			CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString();

			// Create a table client for interacting with the table service
			CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

			_log.LogInformation("Create a Table for the demo");

			// Create a table client for interacting with the table service 
			_table = tableClient.GetTableReference(_tableConfiguration.TableName);

			if (await _table.CreateIfNotExistsAsync())
			{
				_log.LogInformation($"Created Table named: {_tableConfiguration.TableName}");
			}
			else
			{
				_log.LogInformation($"Table {_tableConfiguration.TableName} already exists");
			}

		}

		public async Task<GeneralStatusEnum> InsertOrMergeEntityAsync(AudioEntity entity)
		{
			try
			{
				if (_table is null)
				{
					await CreateTableAsync();
				}
				if (entity == null)
				{
					throw new ArgumentNullException(nameof(entity));
				}
				// Create the InsertOrReplace table operation
				TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);

				// Execute the operation.
				TableResult result = await _table.ExecuteAsync(insertOrMergeOperation);
				var insertedQuote = result.Result as AudioEntity;

				// Get the request units consumed by the current operation. RequestCharge of a TableResult is only applied to Azure CosmoS DB 
				if (result.RequestCharge.HasValue)
				{
					_log.LogInformation($"Request Charge of InsertOrMerge Operation:{ result.RequestCharge}");
				}

				return GeneralStatusEnum.Ok;
			}
			catch (Exception e)
			{
				_log.LogInformation(e.Message);
				return GeneralStatusEnum.ServerError;
			}
		}

		public async Task<AudioEntity> GetEntityAsync(string partitionKey, string rowKey)
		{

			try
			{
				if (_table is null)
				{
					_log.LogInformation("table was null in GetEntityAsync");
					await CreateTableAsync();
				}

				TableOperation insertOrMergeOperation = TableOperation.Retrieve<AudioEntity>(partitionKey, rowKey);

				// Execute the operation.
				TableResult result = await _table.ExecuteAsync(insertOrMergeOperation);
				var image = result.Result as AudioEntity;

				return image;
			}
			catch (Exception e)
			{
				_log.LogInformation(e.Message);
				return null;
			}

		}


		private CloudStorageAccount CreateStorageAccountFromConnectionString()
		{
			CloudStorageAccount storageAccount;
			try
			{
				storageAccount = CloudStorageAccount.Parse(_tableConfiguration.StorageConnectionString);
			}
			catch (FormatException)
			{
				_log.LogInformation(
					"Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
				throw;
			}
			catch (ArgumentException)
			{
				_log.LogInformation(
					"Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");

				throw;
			}

			_log.LogInformation("Success!");
			return storageAccount;
		}


	}
}