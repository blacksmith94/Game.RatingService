using Microsoft.AspNetCore.Mvc;
using System;

namespace Game.RatingService.WebAPI.DTOs
{
	namespace Game.WebAPI.DTOs
	{
		public class RatingDefinition
		{
			[FromRoute(Name = "sessionId")]
			public Guid SessionId { get; set; }

			[FromHeader(Name = "userId")]
			public string UserId { get; set; }

			[FromBody]
			public RatingContent RatingContent { get; set; }
		}
		
		public class RatingContent
		{
			public int Score { get; set; }

			public string? Comment { get; set; }
		}
	}
}
