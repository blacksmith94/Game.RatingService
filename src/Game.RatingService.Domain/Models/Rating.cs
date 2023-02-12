using System;

namespace Game.Domain.Models
{
	public class Rating
	{
		public string UserId { get; set; }

		public Guid SessionId { get; set; }

		public int Score { get; set; }

		public string? Comment { get; set; }

		public DateTime Date { get; set; }
	}
}
