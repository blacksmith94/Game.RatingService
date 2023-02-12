using Game.WebAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Game.WebAPI.Middleware
{
	/// <summary>
	/// Middleware used to catch the exceptions in the request pipeline
	/// </summary>
	public class ExceptionHandler
	{
		private readonly RequestDelegate next;
		private readonly ILogger<ExceptionHandler> logger;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="next"> A function that can process an HTTP request </param>
		/// <param name="logger">Logger</param>
		public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
		{
			this.next = next;
			this.logger = logger;
		}

		/// <summary>
		/// Method that gets called on each request as part of the pipeline
		/// </summary>
		/// <param name="context"> Intercepted http context </param>
		public async Task Invoke(HttpContext context)
		{
			try
			{
				await next.Invoke(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Method that gets called if the service gets an exception, will log and write an ErrorResponse into the context's response.
		/// </summary>
		/// <param name="context"> Intercepted http context </param>
		/// <param name="exception"> The actual exception that was thrown</param>
		private Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
		{
			var message = $"{context.Request.Path} {context.Request.QueryString} {context.Request.Method}";
			logger.LogError(exception, $"Internal server error: {message}");

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			var errorResponse = new ErrorResponse(context.Response.StatusCode, exception.Message, "Internal error");
			return context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
		}
	}
}
