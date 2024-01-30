using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MovieManager.Models;
using Newtonsoft.Json;
using X.PagedList;

namespace MovieManager.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext context;

    public HomeController(
        ILogger<HomeController> logger,
        AppDbContext context
        )
    {
        _logger = logger;
        this.context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.MostViews = await context.Movies.OrderByDescending(p => p.Views).Take(12).ToListAsync();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> Movie(int id)
    {
        var movie = await context.Movies.SingleOrDefaultAsync(p => p.Id == id);

        var visitedMovies = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("movies") ?? "[]");

        if (!visitedMovies.Contains(movie.Id))
        {
            movie.Views++;
            context.Movies.Update(movie);
            await context.SaveChangesAsync();
            visitedMovies.Add(movie.Id);
            HttpContext.Session.SetString("movies", JsonConvert.SerializeObject(visitedMovies));
        }

        return View(movie);
    }

    public async Task<IActionResult> Genre(int id, int? page)
    {
        var genre = await context.Genres.SingleOrDefaultAsync(p => p.Id == id);
        ViewBag.Genre = genre;
        var model = genre.Movies.ToPagedList(page ?? 1, 12);
        return View(model);
    }

    public async Task<IActionResult> Search(string keyword, int? page)
    {
        ViewBag.Keyword = keyword;
        var model = (await context.Movies.Where(p=> p.Name.Contains(keyword)).ToListAsync()).ToPagedList(page ??1 , 12);
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Comment(CommentViewModel model)
    {
        var comment = new Comment
        {
            Date = DateTime.UtcNow,
            Enabled = false,
            Rate = model.Rate,
            Text = model.Text,
            MovieId = model.MovieId,
            UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),

        };

        await context.Comments.AddAsync(comment);
        await context.SaveChangesAsync();
        TempData["success"] = "Yorumunuz tarafımıza ulaşmıştır.";
        return RedirectToAction(nameof(Movie), new { id = model.MovieId });

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
