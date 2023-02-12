using Game.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.Domain.Services
{
	public interface IRatingService
	{
		/// <summary>
		/// Adds a new rating to the database;
		/// <param name="rating"> Rating model to be added to the repository </param>
		/// </summary>
		Task<bool> AddAsync(Rating rating);

		/// <summary>
		/// Gets the latest N ratings from the repository.
		/// </summary>
		/// <param name="ratingScore">Filter the ratings by score, can be null</param>
		/// <returns> The list of the latest ratings </returns>
		List<Rating> GetLatest(int? ratingScore);

		/// <summary>
		/// Checks if the rating already exists
		/// </summary>
		/// <param name="sessionId"> GameSession Id</param>
		/// /// <param name="userId"> User Id</param>
		/// <returns>True or False</returns>
		bool IsUniqueUserAndSession(Guid sessionId, string userId);
	}
}
