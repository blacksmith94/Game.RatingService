using Game.SqlDataAccess.Configs;
using Microsoft.EntityFrameworkCore;

namespace Game.SqlDataAccess
{
	/// <summary>
	/// This class extends the EF DbContext so we can further configurate the bindings between the models and the DB tables.
	/// </summary>
	public class DatabaseContext : DbContext
	{
		public DatabaseContext() : base() { }

		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

		/// <summary>
		/// EF configurations should be applied here.
		/// </summary>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfiguration(new RatingSqlConfig());
		}
	}
}
