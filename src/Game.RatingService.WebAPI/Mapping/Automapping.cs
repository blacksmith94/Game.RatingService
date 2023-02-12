using AutoMapper;
using Game.Domain.Models;
using Game.RatingService.WebAPI.DTOs.Game.WebAPI.DTOs;
using Game.RatingService.WebAPI.Mapping;

namespace Game.WebAPI.Mapping
{
	/// <summary>
	/// This class defines the mapping between DTOs and models.
	/// <para/>
	/// Custom mapping can also be added by using .ConvertUsing<CustomConverter>() where CustomConverter would be a class that implements ITypeConverter;
	/// </summary>
	public class Automapping : Profile
	{
		public Automapping()
		{
			CreateMap<RatingDefinition, Rating>()
				.ConvertUsing<RatingRequestConverter>();
		}
	}
}
