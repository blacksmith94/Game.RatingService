using Game.Configurations;
using Game.Domain.Models;
using Game.RatingService.WebAPI.DTOs.Game.WebAPI.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Game.WebAPI.Tests.Integration
{
	[Collection("Integration")]

	public class RatingServiceTest
	{
		RatingServiceOptions _configuration;
		Request _request;

		public RatingServiceTest(IntegrationFixture fixture)
		{
			_request = fixture.Request;
			_configuration = fixture.Configuration;
		}

		[Fact]
        //In the RatingServiceOptions, the maximum score should not be lower than the minimum score
        public void Test_1_Maximum_Score_Is_Not_Lower_Than_Minimum()
		{
			Assert.False(_configuration.MaxAllowedScore < _configuration.MinAllowedScore);
		}

		[Theory]
		//Should return a 200 code, valid response
		[InlineData("TestUserId1", "860cf0b0-6a78-4dbb-93b6-5214901e9d84", 1, "Test comment")]
		[InlineData("TestUserId2", "860cf0b0-6a78-4dbb-93b6-5214901e9d84", 5, null)]
		public async Task Test_2_Should_Add_Rating_Correctly_On_Rating_Request(string userId, string sessionId, int score, string comment)
		{
			var ratingContent = new RatingContent() { Score = score, Comment = comment };
			var headerDictionary = new HeaderDictionary();
			headerDictionary.Add("userId", userId);
			var ratingResponse = await _request.Post<Rating>($"rating/{sessionId}", ratingContent, headerDictionary);

			Assert.Equal(userId, ratingResponse.UserId);
			Assert.Equal(sessionId, ratingResponse.SessionId.ToString());
			Assert.Equal(ratingContent.Score, ratingResponse.Score);
			Assert.Equal(ratingContent.Comment, ratingResponse.Comment);
		}

		[Theory]
		//No headers request, should return bad request
		[InlineData(1, "Test comment")]
		public async Task Test_3_Should_Return_BadRequest_No_Headers(int rating, string comment)
		{
			var ratingContent = new RatingContent() { Score = rating, Comment = comment };
			var ratingResponse = await _request.Post($"rating/{Guid.NewGuid()}", ratingContent);
			Assert.True(ratingResponse.StatusCode == HttpStatusCode.BadRequest);
		}

		[Theory]
		//Adding the same request twice should return a bad request on the second try
		[InlineData("testUser", "860cf0b0-6a78-4dbb-93b6-5214901e9d84", 1, "Test comment")]
		public async Task Test_4_Should_Return_BadRequest_Repeated_Rating(string userId, string sessionId, int rating, string comment)
		{
			var headerDictionary = new HeaderDictionary();
			headerDictionary.Add("userId", userId);
			var ratingContent = new RatingContent() { Score = rating, Comment = comment };
			await _request.Post($"rating/{sessionId}", ratingContent, headerDictionary);
			var ratingResponse = await _request.Post($"rating/{sessionId}", ratingContent, headerDictionary);
			Assert.True(ratingResponse.StatusCode == HttpStatusCode.Conflict);
		}

		[Theory]
		//Should return a bad request if the sessionId is not a correct Guid
		[InlineData("user1", "5214901e9d84", 1, "Test comment")]
		[InlineData("user2", "cw23d23", 1, "Test comment")]
		public async Task Test_5_Should_Return_BadRequest_Invalid_SessionId(string userId, string sessionId, int rating, string comment)
		{

			var ratingContent = new RatingContent() { Score = rating, Comment = comment };
			var headerDictionary = new HeaderDictionary();
			headerDictionary.Add("userId", userId);
			var ratingResponse = await _request.Post($"rating/{sessionId}", ratingContent, headerDictionary);

			Assert.True(ratingResponse.StatusCode == HttpStatusCode.BadRequest);
		}

		[Theory]
		//The obtained ratings should match the specified rating filter
		[InlineData("test1", 1)]
		[InlineData("test2", 2)]
		[InlineData("test3", 3)]
		[InlineData("test4", 4)]
		[InlineData("test5", 5)]
		public async Task Test_6_Rating_Filter_Should_Filter_Correctly(string userId, int scoreFilter)
		{
			var headerDictionary = new HeaderDictionary();
			headerDictionary.Add("userId", userId);
			for (int rating = 1; rating < 5; rating++)
			{
				var ratingContent = new RatingContent() { Score = rating };
				await _request.Post($"rating/{Guid.NewGuid()}", ratingContent, headerDictionary);
			}

			//Request the rating list
			var ratings = await _request.Get<List<Rating>>($"rating?score={scoreFilter}");
			foreach (var rating in ratings)
			{
				Assert.True(rating.Score == scoreFilter);
			}
		}

		[Theory]
		//Should return a BadRequest when the rating is invalid
		[InlineData("testName1", "860cf0b0-6a78-4dbb-93b6-5214901e9d84", -1000, "Test comment")]
		[InlineData("testName2", "860cf0b0-6a78-4dbb-93b6-5214901e9d84", 1000, null)]
		public async Task Test_7_Should_Return_Bad_Request_On_Wrong_Rating(string userId, string sessionId, int rating, string comment)
		{
			if (rating < _configuration.MinAllowedScore || rating > _configuration.MaxAllowedScore)
			{
				var ratingContent = new RatingContent() { Score = rating, Comment = comment };
				var headerDictionary = new HeaderDictionary();
				headerDictionary.Add("userId", userId);
				var httpResponse = await _request.Post($"rating/{sessionId}", ratingContent, headerDictionary);
				Assert.True(httpResponse.StatusCode == HttpStatusCode.BadRequest);
			}
		}

		[Fact]
		//The number of obtained ratings should be equal or lower than the maximum configured
		public async Task Test_8_Should_Return_Max_Or_Less_Ratings()
		{
			var getResponse = await _request.Get<List<Rating>>($"rating");
			Assert.True(getResponse.Count <= _configuration.MaxLatestRating);
		}
	}
}
