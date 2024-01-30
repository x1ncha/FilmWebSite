using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MovieManager.Data;

public class AppUser : IdentityUser<int>
{
    public enum Genders
    {
        [Display(Name = "Erkek")]
        Male,
        [Display(Name = "Kadın")]
        Female
    }

   

        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public Genders? Gender { get; set; }
        public bool Enabled { get; set; } = true;

        [NotMapped]
        public string Name => $"{FirstName} {LastName}";
    public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
}
public class AppUserEntityTypeConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder
            .HasIndex(p => new { p.FirstName, p.LastName })
            .IsUnique(false);

        builder
            .Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(450);

        builder
            .Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(450);
        builder
        .HasMany(p => p.Comments)
        .WithOne(p => p.CreatorUser)
        .HasForeignKey(p => p.UserId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}