using AudioProcessor.Data;
using AudioProcessor.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Http;
using PhotoProcessor.Functions;
using System;
using System.Linq;
using System.Reflection;
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
			if (serviceConfig is null) { throw new Exception(); }

			_ = (IConfiguration)serviceConfig.ImplementationInstance;

			
			builder.Services.AddLogging();
			builder.Services.AddHttpClient();

			builder.Services.AddOptions<AzStorageConfiguration>()
				.Configure<IConfiguration>((settings, configuration) =>
				{
					configuration.GetSection("AzStorageConfiguration").Bind(settings);
				});

			builder.Services.AddOptions<SpeechConfiguration>()
				.Configure<IConfiguration>((settings, configuration) =>
				{
					configuration.GetSection("SpeechConfiguration").Bind(settings);
				});


			builder.Services.AddSingleton<ITableDbContext, TableDbContext>();

			builder.Services.AddSingleton<IBlobContext, BlobContext>();

			builder.Services.AddSingleton<IDataRepository, DataRepository>();

			builder.Services.AddSingleton<IDownloadService, DownloadService>();

			builder.Services.AddSingleton<TranscriptionService>();

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