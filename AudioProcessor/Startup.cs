using AudioProcessor.Data;
using AudioProcessor.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhotoProcessor.Functions;
using System;
using System.Linq;
using System.Reflection;
//https://damienbod.com/2020/07/12/azure-functions-configuration-and-secrets-management/
[assembly: FunctionsStartup(typeof(Startup))]
namespace PhotoProcessor.Functions
{
	public class Startup : FunctionsStartup
	{

		public override void Configure(IFunctionsHostBuilder builder)
		{
			// ------------------ default configuration initialise ------------------
			var serviceConfig = builder.Services.FirstOrDefault(s => s.ServiceType == typeof(IConfiguration));
			if (serviceConfig != null)
			{
				_ = (IConfiguration)serviceConfig.ImplementationInstance;
			}

			builder.Services.AddLogging();

			builder.Services.AddOptions<BlobConfiguration>()
				.Configure<IConfiguration>((settings, configuration) =>
				{
					configuration.GetSection("BlobConfiguration").Bind(settings);
				});


			// ------------------ TableStorageDb initialise ------------------
			//ITableConfiguration tableConfig = new TableConfiguration
			//{
			//	ConnectionString = Environment.GetEnvironmentVariable("StorageConnectionString"),
			//	TableName = Environment.GetEnvironmentVariable("TableName")
			//};

			//builder.Services.AddSingleton(tableConfig);
			//builder.Services.AddSingleton<ITableDbContext, TableDbContext>();

			builder.Services.AddScoped<IBlobContext, BlobContext>();

			builder.Services.AddScoped<IDataRepository, DataRepository>();

			//IPhotoApiSettings photoApiSettings = new PhotoApiSettings
			//{
			//	PrivateKey = Environment.GetEnvironmentVariable("PhotoApiSettingsPrivateKey"),
			//	AppId = Environment.GetEnvironmentVariable("PhotoApiSettingsAppId")

			//};

			//builder.Services.AddSingleton(photoApiSettings);
			//builder.Services.AddScoped<IPhotoService, PhotoFiddler>();
			//builder.Services.AddScoped<IDownloadService, DownloadService>();

		}

		public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
		{
			
			builder.ConfigurationBuilder
			   .SetBasePath(Environment.CurrentDirectory)
			   .AddJsonFile("local.settings.json", true)
			   .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
			   .AddEnvironmentVariables()
			   .Build();

		}
	}
}