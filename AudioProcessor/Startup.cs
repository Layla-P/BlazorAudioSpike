using AudioProcessor.Data;
using AudioProcessor.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhotoProcessor.Functions;
using System;
using System.Linq;
using System.Reflection;
using Transcription;
using Transcription.Common.Dtos;
using Transcription.Helpers.Interfaces;
using Transcription.Helpers;
using Transcription.Common.Helpers;
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

			builder.Services.AddOptions<TranscriptionAccount>()
				.Configure<IConfiguration>((settings, configuration) =>
				{
					configuration.GetSection("TranscriptionAccount").Bind(settings);
				});



			builder.Services.AddHttpClient();

			builder.Services.AddHttpClient<IServiceHelpers, ServiceHelper>();

			builder.Services.AddSingleton<ITranscriptionService, TranscriptionService>();

			builder.Services.AddSingleton<ITableDbContext, TableDbContext>();

			builder.Services.AddSingleton<IBlobContext, BlobContext>();

			builder.Services.AddSingleton<IDataRepository, DataRepository>();

			builder.Services.AddSingleton<IDownloadService, DownloadService>();

		}

		public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
		{
			//if (System.Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Development")
			//{
			//	builder.ConfigurationBuilder.AddUserSecrets<Startup>();
			//}
			builder.ConfigurationBuilder
			   .SetBasePath(Environment.CurrentDirectory)
			   .AddJsonFile("local.settings.json", true)
			   //.AddUserSecrets(Assembly.GetExecutingAssembly(), true)
			   .AddUserSecrets<Startup>()
			   .AddEnvironmentVariables()
			   .Build();

		}
	}
}