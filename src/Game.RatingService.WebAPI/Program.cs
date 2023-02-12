using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Threading.Tasks;

namespace Game.WebAPI
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

			try
			{
				logger.Info("Rating Service Init");

				var host = Host.CreateDefaultBuilder(args)
							   .ConfigureWebHostDefaults(webHostBuilder =>
							   {
								   webHostBuilder.ConfigureLogging(l => l.ClearProviders())
												 .ConfigureLogging(l => l.SetMinimumLevel(LogLevel.Information))
												 .ConfigureLogging(l => l.AddConsole())
												 .UseNLog()
												 .UseKestrel()
												 .UseStartup<Startup>()
												 .UseUrls("http://0.0.0.0:5001/");
							   })
							   .Build();

				await host.RunAsync();
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Stopped program because of exception");
				throw;
			}
			finally
			{
				NLog.LogManager.Shutdown();
			}
		}
	}
}
