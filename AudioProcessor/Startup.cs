using AudioProcessor.Data;
using AudioProcessor.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhotoProcessor.Functions;
using System;
using System.Linq;
using System.Reflection;
using AssemblyAi;
using AssemblyAi.Common.Dtos;
using AssemblyAi.Helpers.Interfaces;
using AssemblyAi.Helpers;
using AssemblyAi.Common.Enums;
using AssemblyAi.Common.Helpers;
using System.Text.Json;
using AudioProcessor.Services;

[assembly: FunctionsStartup(typeof(Startup))]
namespace PhotoProcessor.Functions
{
	public class Startup : FunctionsStartup
	{

		public override void Configure(IFunctionsHostBuilder builder)
		{
			
			// ------------------ default configuration initialise ------------------
			var serviceConfig = builder.Services.FirstOrDefault(s => s.ServiceType == typeof(IConfiguration));
			if(serviceConfig is null) { throw new Exception(); }

			_ = (IConfiguration)serviceConfig.ImplementationInstance;

			
			builder.Services.AddLogging();
			

			builder.Services.AddOptions<AzStorageConfiguration>()
				.Configure<IConfiguration>((settings, configuration) =>
				{
					configuration.GetSection("AzStorageConfiguration").Bind(settings);
				});

			builder.Services.AddOptions<AssemblyAiAccount>()
				.Configure<IConfiguration>((settings, configuration) =>
				{
					configuration.GetSection("AssemblyAiAccount").Bind(settings);
				});



			// ------------------ TableStorageDb initialise ------------------
			//ITableConfiguration tableConfig = new TableConfiguration
			//{
			//	ConnectionString = Environment.GetEnvironmentVariable("StorageConnectionString"),
			//	TableName = Environment.GetEnvironmentVariable("TableName")
			//};

			//builder.Services.AddSingleton(tableConfig);
			builder.Services.AddSingleton(sp => new JsonSerializerOptions
			{
				Converters =
				{
					new EnumConvertor<AcousticModelEnum>(),
					new EnumConvertor<BoostParamEnum>()
				}
			});


			builder.Services.AddHttpClient();
			builder.Services.AddHttpClient<IServiceHelpers, ServiceHelper>();
			builder.Services.AddSingleton<IAssemblyAiService, AssemblyAiService>();

			builder.Services.AddSingleton<ITableDbContext, TableDbContext>();

			builder.Services.AddSingleton<IBlobContext, BlobContext>();

			builder.Services.AddSingleton<IDataRepository, DataRepository>();

			//IPhotoApiSettings photoApiSettings = new PhotoApiSettings
			//{
			//	PrivateKey = Environment.GetEnvironmentVariable("PhotoApiSettingsPrivateKey"),
			//	AppId = Environment.GetEnvironmentVariable("PhotoApiSettingsAppId")

			//};

			//builder.Services.AddSingleton(photoApiSettings);
			//builder.Services.AddScoped<IPhotoService, PhotoFiddler>();
			builder.Services.AddSingleton<IDownloadService, DownloadService>();

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