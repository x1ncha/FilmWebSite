using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieManager.Data;

namespace MovieManager;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
{

    public AppDbContext(DbContextOptions options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Genre>(
            builder =>
            {
                builder
                .HasIndex(p => new { p.Name })
                .IsUnique(true);

                builder
                .Property(p => p.Name)
                .HasMaxLength(450)
                .IsRequired(true);

                builder
                .HasMany(p=>p.Movies)
                .WithOne(p=>p.Genre)
                .HasForeignKey(p=>p.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            });
        modelBuilder.Entity<AppUser>(
            builder =>
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

            });

        modelBuilder.Entity<Movie>(
            builder =>
            {
                builder
                .HasIndex(p => new { p.Name })
                .IsUnique(false);

                builder
                .Property(p => p.Name)
                .HasMaxLength(450)
                .IsRequired(true);

                builder
                .Property(p=>p.Image)
                .IsUnicode(false);

                builder
           .HasMany(p => p.Comments)
           .WithOne(p => p.Movie)
           .HasForeignKey(p => p.MovieId)
           .OnDelete(DeleteBehavior.Cascade);
            });

        base.OnModelCreating(modelBuilder);
    }


    public virtual DbSet<Genre> Genres { get; set; }
    public virtual DbSet<Movie> Movies { get; set; }
    public virtual DbSet<AppUser> AppUsers { get; set; }
    public virtual DbSet<AppRole> AppRoles { get; set; }
    public virtual DbSet<Comment> Comments { get; set; }

}