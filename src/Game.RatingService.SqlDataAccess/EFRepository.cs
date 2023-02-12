using Game.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Game.SqlDataAccess
{
	/// <summary>
	/// This class is used to create a repository of a given model. It is registered as a service, implementing the domain's IRepository.
	/// </summary>
	public class EFRepository<T> : IRepository<T> where T : class
	{
		/// <summary>
		/// DbSet used to maque querys
		/// </summary>
		private readonly DbSet<T> dbSet;

		/// <summary>
		/// The actual context of the database registered at startup
		/// </summary>
		private readonly DatabaseContext dbContext;

		/// <summary>
		/// Class constructor
		/// <param name="dbContext">DbContext being injected</param>
		/// </summary>
		public EFRepository(DatabaseContext dbContext)
		{
			this.dbSet = dbContext.Set<T>();
			this.dbContext = dbContext;
		}

		/// <summary>
		/// Adds a given model item to its table
		/// <param name="item">Model item to be added to the dbSet</param>
		/// </summary>
		public async Task Add(T item) => await dbSet.AddAsync(item);

		/// <summary>
		/// Returns an IQueriable of the dbSet that we can use to make queries as no tracking, since it's going to be used only to read registers from the database.
		/// </summary>
		public IQueryable<T> Query => dbSet.AsNoTracking();

		/// <summary>
		/// Saves the dbContext, all changes made to the database will be saved.
		/// </summary>
		public async Task Save() => await dbContext.SaveChangesAsync();
	}
}
