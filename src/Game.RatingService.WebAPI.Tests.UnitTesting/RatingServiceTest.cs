using Game.Configurations;
using Game.Domain;
using Game.Domain.Models;
using Microsoft.Extensions.Options;
using Moq;
using System;
using Xunit;

namespace Game.WebAPI.Tests.UnitTesting
{
	public class RatingServiceTest
	{
		//Tests that the rating service is not null
		[Fact]
		public void Test_1_RatingService_Should_Not_Be_Null()
		{
			var ratingRepoMock = new Mock<IRepository<Rating>>();
			var configMock = new Mock<IOptions<RatingServiceOptions>>();

			var RatingService = new Domain.Services.RatingService(ratingRepoMock.Object, configMock.Object);

			Assert.NotNull(RatingService);
		}

		//Tests that the rating service looks for a repeated rating correctly
		[Fact]
		public void Test_2_Should_Be_Unique_Session()
		{
			var ratingRepoMock = new Mock<IRepository<Rating>>();
			var configMock = new Mock<IOptions<RatingServiceOptions>>();

			var ratingService = new Domain.Services.RatingService(ratingRepoMock.Object, configMock.Object);
			var rating = new Rating()
			{
				Comment = "",
				Score = 5,
				SessionId = Guid.NewGuid(),
				UserId = "User123",
				Date = DateTime.Now
			};
			Assert.True(ratingService.IsUniqueUserAndSession(rating.SessionId, rating.UserId));
		}
	}
}
