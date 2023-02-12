using Game.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using Xunit;

namespace Game.WebAPI.Tests.Integration
{
	[CollectionDefinition("Integration")]
	public sealed class IntegrationFixture : IDisposable, ICollectionFixture<IntegrationFixture>
	{
		private readonly IHost _host;
		private readonly TestServer _server;
		private readonly HttpClient _client;

		public Request Request { get; }
		public RatingServiceOptions Configuration { get; }

		public IntegrationFixture()
		{
			Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
			var hostBuilder = new HostBuilder()
						.ConfigureWebHost(webHost =>
						{
							webHost.UseTestServer();
							webHost.UseStartup<Startup>();
						});

			var configBuilder = new ConfigurationBuilder()
						 .AddJsonFile("appsettings.Test.json", optional: true, reloadOnChange: true);
			var configRoot = configBuilder.Build();

			Configuration = configRoot.GetSection("RatingServiceOptions").Get<RatingServiceOptions>();

			_host = hostBuilder.Start();
			_server = _host.GetTestServer();
			_client = _server.CreateClient();

			Request = new Request(_client);
		}

		public void Dispose()
		{
			_client.Dispose();
			_server.Dispose();
		}
	}
}
