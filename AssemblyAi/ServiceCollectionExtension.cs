using AssemblyAi.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using AssemblyAi.Common.Dtos;
using AssemblyAi.Common.Enums;
using AssemblyAi.Common.Helpers;
using AssemblyAi.Helpers.Interfaces;

namespace AssemblyAi
{
	public static class ServiceCollectionExtension
	{

		public static IServiceCollection AddAssemblyAi(
			this IServiceCollection services,
			Action<AssemblyAiAccount> accountOptions)
		{
			
			var assemblyAi = new AssemblyAiAccount();
			accountOptions.Invoke(assemblyAi);
			services.AddSingleton(assemblyAi);
			services.AddSingleton(sp => new JsonSerializerOptions
			{
				Converters =
				{
					new EnumConvertor<AcousticModelEnum>(),
					new EnumConvertor<BoostParamEnum>()
				}
			});
			services.AddHttpClient<IServiceHelpers, ServiceHelper>();
			services.AddHttpClient<AssemblyAiService>();
			return services;
		}
	}
}
