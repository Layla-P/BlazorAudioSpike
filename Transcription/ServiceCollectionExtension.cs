using Transcription.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using Transcription.Common.Dtos;
using Transcription.Common.Helpers;
using Transcription.Helpers.Interfaces;

namespace Transcription
{
	public static class ServiceCollectionExtension
	{

		public static IServiceCollection AddTranscription(
			this IServiceCollection services,
			Action<TranscriptionAccount> accountOptions)
		{
			
			var transcription = new TranscriptionAccount();
			accountOptions.Invoke(transcription);
			services.AddSingleton(transcription);			
			services.AddHttpClient<IServiceHelpers, ServiceHelper>();
			services.AddHttpClient<TranscriptionService>();
			return services;
		}
	}
}
