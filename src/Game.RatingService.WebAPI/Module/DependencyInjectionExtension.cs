using FluentValidation;
using Game.Domain.Services;
using Game.RatingService.WebAPI.DTOs.Game.WebAPI.DTOs;
using Game.WebAPI.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Game.WebAPI.Module
{
	public static class DependencyInjectionExtension
	{
		public static void AddDomainServices(this IServiceCollection services)
		{
			services.AddScoped<IRatingService, Game.Domain.Services.RatingService>();
			services.AddTransient<IValidator<RatingDefinition>, RatingDefinitionValidator>();
		}
	}
}
