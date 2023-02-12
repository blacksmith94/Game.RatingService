using Game.Domain;
using Game.Domain.Models;
using Game.SqlDataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Game.RatingService.SqlDataAccess.Module
{
    public static class DependencyInjectionExtension
    {
        public static void AddSql(this IServiceCollection services, IConfiguration configuration)
        {
            var sqlConfig = configuration.GetSection("Sql").Get<SqlOptions>();

            var sqlHost = Environment.GetEnvironmentVariable("SQL_HOST");
            if (!string.IsNullOrEmpty(sqlHost))
            {
                // Replace the server name with the environment variable value
                var connectionStringBuilder = new SqlConnectionStringBuilder(sqlConfig.DbConnectionString);
                connectionStringBuilder.DataSource = sqlHost;
                sqlConfig.DbConnectionString = connectionStringBuilder.ConnectionString.Replace("Data Source=", "Server=");
            }

            //Add DbContext using SQL Server
            services.AddDbContext<DatabaseContext>((serviceProvider, optionsBuilder) =>
			{
				optionsBuilder.UseSqlServer(sqlConfig.DbConnectionString);

			}, ServiceLifetime.Transient);
			
            services.AddScoped<IRepository<Rating>, EFRepository<Rating>>();
		}
    }
}
