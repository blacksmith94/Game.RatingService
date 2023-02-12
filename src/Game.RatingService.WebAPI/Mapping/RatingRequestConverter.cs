using AutoMapper;
using System;
using Game.Domain.Models;
using Game.RatingService.WebAPI.DTOs.Game.WebAPI.DTOs;

namespace Game.RatingService.WebAPI.Mapping
{
	public class RatingRequestConverter : ITypeConverter<RatingDefinition, Rating>
	{
		public Rating Convert(RatingDefinition source, Rating destination, ResolutionContext context)
		{
			return new Rating()
			{
				UserId = source.UserId,
				SessionId = source.SessionId,
				Score = source.RatingContent.Score,
				Comment = source.RatingContent.Comment,
				Date = DateTime.Now
			};
		}
	}
}
