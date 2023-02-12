using Game.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.SqlDataAccess.Configs
{
	/// <summary>
	/// This class lets Entity Framework map the table design to the Rating model, all Rating table schema specifications should be defined here.
	/// </summary>
	public class RatingSqlConfig : IEntityTypeConfiguration<Rating>
	{
		public void Configure(EntityTypeBuilder<Rating> builder)
		{
			builder.ToTable("rating");

			builder.Property(f => f.UserId).HasColumnName("userId");
			builder.Property(f => f.SessionId).HasColumnName("sessionId");
			builder.Property(f => f.Score).HasColumnName("score");
			builder.Property(f => f.Comment).HasColumnName("comment");
			builder.Property(f => f.Date).HasColumnName("date");

			builder.HasKey(f => new { f.SessionId, f.UserId });

			builder.HasIndex(f => f.Score);
		}
	}
}
