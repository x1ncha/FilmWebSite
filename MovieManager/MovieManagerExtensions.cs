using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieManager.Data;
using System.Security.Claims;
using System.Text.RegularExpressions;
using X.PagedList.Web.Common;

namespace MovieManager
{
    public static class MovieManagerExtensions
    {
        public static PagedListRenderOptions PagedListOptions => new PagedListRenderOptions
        {
            ActiveLiElementClass = "active",
            LiElementClasses = new[] { "page-item" },
            PageClasses = new[] { "page-link" }
        };
        public static IApplicationBuilder UseMovieWeb(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            context.Database.Migrate();

            new[]
            {
            new AppRole{ Name = "Administrators"},
            new AppRole{ Name = "ProductAdministrators"},
            new AppRole{ Name = "OrderAdministrators"},
            new AppRole{ Name = "Members"},
        }
            .ToList()
            .ForEach(role =>
            {
                if (!roleManager.RoleExistsAsync(role.Name).Result)
                {
                    var result = roleManager.CreateAsync(role).Result;
                }
            });

            var user = new AppUser
            {
                FirstName = configuration.GetValue<string>("DefaultUser:FirstName"),
                LastName = configuration.GetValue<string>("DefaultUser:LastName"),
                UserName = configuration.GetValue<string>("DefaultUser:Email"),
                Email = configuration.GetValue<string>("DefaultUser:Email"),
                EmailConfirmed = true
            };

            userManager.CreateAsync(user, configuration.GetValue<string>("DefaultUser:Password")).Wait();
            userManager.AddToRoleAsync(user, "Administrators").Wait();
            userManager.AddClaimAsync(user, new Claim("Name", user.Name)).Wait();
            return app;
        }

        public static string ToSafeUrlString(this string text) => Regex.Replace(string.Concat(text.Where(p => char.IsLetterOrDigit(p) || char.IsWhiteSpace(p))), @"\s+", "-");
    }
}
