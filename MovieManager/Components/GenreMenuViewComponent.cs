using Microsoft.AspNetCore.Mvc;

namespace MovieManager.Components
{
    public class GenreMenuViewComponent : ViewComponent
    {
        private readonly AppDbContext context;

        public GenreMenuViewComponent(
            AppDbContext context
            )
        {
            this.context = context;
        }

        public IViewComponentResult Invoke()
        {
            var model = context.Genres.ToList();
            return View(model);
        }
    }
}
