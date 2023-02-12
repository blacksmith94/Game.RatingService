using FluentValidation.Results;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Game.WebAPI.DTOs
{
	/// <summary>
	/// Class used to return information about an error that occurred in the service.
	/// </summary>
	public class ErrorResponse
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="code"> Error code </param>
		/// <param name="message"> Error message </param>
		/// <param name="code"> Error Status </param>
		public ErrorResponse(int code, string message, string status)
		{
			Code = code;
			Message = message;
			Status = status;
		}

		/// <summary>
		/// Overloaded class constructor in case we want to add the validation errors
		/// </summary>
		/// <param name="code"> Error code </param>
		/// <param name="message"> Error message </param>
		/// <param name="code"> Error Status </param>
		/// <param name="errors"> ValidationFailure List</param>
		[JsonConstructor]
		public ErrorResponse(int code, string message, string status, IList<ValidationFailure>? errors)
		{
			Code = code;
			Message = message;
			Status = status;
			Errors = new List<SingleErrorResponse>();
			if (errors != null)
			{
				foreach (var e in errors)
				{
					Errors.Add(new SingleErrorResponse()
					{
						Code = e.ErrorCode,
						Message = e.ErrorMessage
					});
				}
			}
		}

		/// <summary>
		/// Error code
		/// </summary>
		/// <example>400</example>
		public int Code { get; set; }

		/// <summary>
		/// Error message
		/// </summary>
		/// <example>Could not process request</example>
		public string Message { get; set; }

		/// <summary>
		/// List of validation errors
		/// </summary>
		/// <example>[{ Code = "1", Message = "Invalid rating" }]</example>
		public List<SingleErrorResponse> Errors { get; set; }

		/// <summary>
		/// Error status
		/// </summary>
		/// <example>BadRequest</example>
		public string Status { get; internal set; }
	}


	/// <summary>
	/// Class used to describe inner validation errors
	/// </summary>
	public class SingleErrorResponse
	{
		/// <summary>
		/// Error code 
		/// </summary>
		/// <example>1</example>
		public string Code { get; set; }

		/// <summary>
		/// Error message
		/// </summary>
		/// <example>Invalid rating</example>
		public string Message { get; set; }
	}
}
