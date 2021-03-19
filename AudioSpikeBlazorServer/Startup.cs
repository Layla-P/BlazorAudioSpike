using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AudioSpikeBlazorServer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.StaticFiles;
using Blazored.LocalStorage;
using AssemblyAi.Common.Helpers;
using AssemblyAi.Common.Enums;
using System.Text.Json;

namespace AudioSpikeBlazorServer
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddHttpClient("AudioProcessor", client =>
			{
				client.BaseAddress = new Uri("http://localhost:7071/api/");
			});
			services.AddRazorPages();
			services.AddServerSideBlazor();
			services.AddBlazoredLocalStorage();

			services.AddSingleton(sp => new JsonSerializerOptions
			{
				Converters =
				{
					new EnumConvertor<AcousticModelEnum>(),
					new EnumConvertor<BoostParamEnum>()
				}
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			// we need both UseStaticFiles as the parameterless constructor adds everything in wwwroot
			//https://docs.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-5.0
			app.UseStaticFiles();
			// todo use an extension in the mic component to initialise this
			var provider = new FileExtensionContentTypeProvider();
			provider.Mappings[".mem"] = "text/html";

			app.UseStaticFiles(
				new StaticFileOptions
				{
					ContentTypeProvider = provider
				});

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");
			});
		}
	}
}
