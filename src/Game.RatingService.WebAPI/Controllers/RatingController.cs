using AutoMapper;
using Game.Domain.Models;
using Game.Domain.Services;
using Game.RatingService.WebAPI.DTOs.Game.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Game.WebAPI.Controllers
{
	[Route("api/[Controller]")]
	[ApiController]
	public class RatingController : Controller
	{
		private readonly ILogger<RatingController> _logger;
		private readonly IMapper _mapper;
		private readonly IRatingService _ratingService;

		public RatingController(ILogger<RatingController> logger, IMapper mapper, IRatingService ratingService)
		{
			_logger = logger;
			_mapper = mapper;
			_ratingService = ratingService;
		}


		/// <summary>
		/// Gets latest N ratings, can be filtered by rating.
		/// </summary>
		/// <param name="score">Rating filter value, between the configured min and max rating.</param>
		/// <returns code="200">The list of the latest ratings</returns>
		[HttpGet()]
		[ProducesResponseType(typeof(List<Rating>), (int)HttpStatusCode.OK)]
		public ActionResult<List<Rating>> GetLatestRating(int? score)
		{
			var model = _ratingService.GetLatest(score);
			var response = _mapper.Map<List<Rating>, List<Rating>>(model);
			_logger.LogInformation($"Get latest ratings");

			return Ok(response);
		}

		/// <summary>
		/// Adds a new rating.
		/// </summary>
		/// <param name="definition">rating definition</param>
		/// <returns code="200">A copy of the created rating</returns>
		/// <returns code="400">Bad Request</returns>
		[HttpPost("{sessionId}")]
		[ProducesResponseType(typeof(Rating), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.Conflict)]
		public async Task<ActionResult<Rating>> PostRating([FromQuery] RatingDefinition definition)
		{
			//Map model
			var model = _mapper.Map<RatingDefinition, Rating>(definition);

			//Add to db
			var registryAdded = await _ratingService.AddAsync(model);
			if (!registryAdded)
				return Conflict("Can't add a rating twice");

			_logger.LogInformation($"Added rating from user '{model.UserId}' with session '{model.SessionId}'");

			return Ok(model);
		}
	}
}