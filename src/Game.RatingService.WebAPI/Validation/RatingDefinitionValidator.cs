using FluentValidation;
using Game.Configurations;
using Game.RatingService.WebAPI.DTOs.Game.WebAPI.DTOs;
using Microsoft.Extensions.Options;

namespace Game.WebAPI.Validation
{
	/// <summary>
	/// This class will validate a RatingDefinition payload using Fluent Validator, 
	/// </summary>
	public class RatingDefinitionValidator : AbstractValidator<RatingDefinition>
	{
		private readonly RatingServiceOptions _options;
		/// <summary>
		/// Constructor
		/// <param name="configuration"> Injected the service configuration </param>
		/// </summary>
		public RatingDefinitionValidator(IOptions<RatingServiceOptions> configuration)
		{
			_options = configuration.Value;
			
			RuleFor(ratingDefinition => ratingDefinition.UserId)
				.NotNull().NotEmpty()
				.WithMessage("User id can't be empty")
				.WithErrorCode("1");
			
			RuleFor(ratingDefinition => ratingDefinition.RatingContent)
				.NotNull()
				.WithMessage("Rating content can't be null")
				.WithErrorCode("2");
			
			When(ratingDefinition => ratingDefinition.RatingContent != null, () =>
			{
				RuleFor(ratingDefinition => ratingDefinition.RatingContent).Must((ratingDefinitionContent)
					=> ValidRating(ratingDefinitionContent.Score))
						.WithMessage($"Rating must be an integer from {_options.MinAllowedScore} to {_options.MaxAllowedScore}")
						.WithErrorCode("3");
			});
		}

		/// <summary>
		/// Validates that rating is below or equal to MinAllowedScore and above or equal to MaxAllowedScore.
		/// <param name="rating"> Rating to validate </param>
		/// </summary>
		private bool ValidRating(int rating)
		{
			return (rating >= _options.MinAllowedScore && rating <= _options.MaxAllowedScore);
		}
	}
}
