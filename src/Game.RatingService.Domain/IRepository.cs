using System.Linq;
using System.Threading.Tasks;

namespace Game.Domain
{
	/// <summary>
	/// Generic repository interface. Will be implemented by EFRepository of a given model.
	/// It lets the Domain use the SqlDataAccess module without having to reference it, keeping the Domain completely isolated, thus following a DDD architecture (Domain Driven Design)
	/// </summary>
	public interface IRepository<T>
	{
		/// <summary>
		/// This function will implement adding a register to the database
		/// </summary>
		Task Add(T entity);

		/// <summary>
		/// This function will implement returning an IQueryable that can be used to make querys to the database.
		/// </summary>
		IQueryable<T> Query { get; }

		/// <summary>
		/// This function will implement saving the changes done.
		/// </summary>
		Task Save();
	}
}
