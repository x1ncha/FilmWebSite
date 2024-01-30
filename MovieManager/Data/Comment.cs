using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieManager.Data;
namespace MovieManager

{
    public class Comment : BaseEntity
    {

        public string? Text { get; set; }

        public int Rate { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public int MovieId { get; set; }

        public virtual Movie? Movie { get; set; }

    }

    public class CommentEntityTypeConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {


        }
    }
}
