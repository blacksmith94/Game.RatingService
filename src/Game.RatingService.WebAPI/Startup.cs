using FluentValidation.AspNetCore;
using Game.Configurations;
using Game.RatingService.SqlDataAccess.Module;
using Game.SqlDataAccess;
using Game.WebAPI.Middleware;
using Game.WebAPI.Module;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Game.WebAPI
{
	public class Startup
	{
		private readonly IConfiguration _configuration;
	
		public Startup(IWebHostEnvironment env)
		{
			var builder = new ConfigurationBuilder()
						 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
						 .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
						 .AddEnvironmentVariables();
			_configuration = builder.Build();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			//Addind service options
			var ratingOptions = _configuration.GetSection("RatingServiceOptions");
            services.Configure<RatingServiceOptions>(ratingOptions);

			//All validators will be registered automatically (Fluent Validation)
			services.AddMvc()
					.AddFluentValidation(mvcConfig => mvcConfig.RegisterValidatorsFromAssemblyContaining<Startup>())
					.AddNewtonsoftJson();

			//Add Swagger documentation, this will let the developers see the API documentation in an intertactive web application
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Rating Service",
					Version = "v1",
					Description = "This service allows users to rate their game session by giving a score from 1 to 5, they can also leave a comment"
				});
			});

			//Add automapper service, this will let developers define DTO <-> Model conversion in a scalable way.
			services.AddAutoMapper(typeof(Startup));

			services.AddControllers();

			services.Configure<KestrelServerOptions>(options =>
			{
				options.AllowSynchronousIO = true;
			});
			
			services.AddSql(_configuration);
			
			services.AddDomainServices();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseContext dbContext)
		{
			//Check db integrity
			if (env.IsEnvironment("Test"))
			{
				dbContext.Database.EnsureDeleted();
			}
			dbContext.Database.EnsureCreated();

			//Middleware
			if (env.IsEnvironment("Debug"))
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseMiddleware<ExceptionHandler>();
			//app.UseExceptionHandler("/Error");
			app.UseRouting();
			app.UseEndpoints(e => e.MapControllers());

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Game Service");
			});
		}
	}
}
