using Game.Configurations;
using Game.Domain.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Domain.Services
{
	public class RatingService : IRatingService
	{
		private readonly IRepository<Rating> repository;
		private readonly RatingServiceOptions configuration;

		public RatingService(IRepository<Rating> repository,
							   IOptions<RatingServiceOptions> configuration)
		{
			this.repository = repository;
			this.configuration = configuration.Value;
		}

		public async Task<bool> AddAsync(Rating rating)
		{
			bool result = false;

			if (IsUniqueUserAndSession(rating.SessionId, rating.UserId))
			{
				await repository.Add(rating);
				await repository.Save();
				result = true;
			}
			return await Task.FromResult(result);
		}

		public List<Rating> GetLatest(int? ratingScore)
		{
			var query = this.repository.Query;
			if (ratingScore != null)
			{
				query = query.Where(r => r.Score == ratingScore);
			}
			return query.Skip(Math.Max(0, query.Count() - configuration.MaxLatestRating)).ToList();
		}

		public bool IsUniqueUserAndSession(Guid sessionId, string userId)
		{
			var query = this.repository.Query;

			var ratingCount = query.Where(r => r.SessionId == sessionId)
			.Where(r => r.UserId == userId)
			.Count();
			if (ratingCount > 0)
			{
				return false;
			}

			return true;
		}
	}
}