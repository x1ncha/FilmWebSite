using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieManager.Data;

namespace MovieManager.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GenresController : Controller
    {
        private readonly AppDbContext context;

        public GenresController(AppDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var model = context.Genres.OrderBy(p => p.Name).ToList();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Genre model)
        {
            context.Genres.Add(model);
            try
            {
                context.SaveChanges();
                TempData["success"] = "Tür ekleme işlemi başarıyla tamamlanmıştır";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["error"] = "Aynı isimli bir başka kayıt olduğundan kayıt işlemi tamamlanamıyor!";
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            var model = context.Genres.Find(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Genre model)
        {
            context.Genres.Update(model);
            try
            {
                context.SaveChanges();
                TempData["success"] = "Tür güncelleme işlemi başarıyla tamamlanmıştır";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["error"] = "Aynı isimli bir başka kayıt olduğundan kayıt işlemi tamamlanamıyor!";
                return View(model);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await context.Genres.FindAsync(id);
            if (model != null)
            {
                context.Genres.Remove(model);
            }
            try
            {
                context.SaveChanges();
                TempData["success"] = "Tür silme işlemi başarıyla tamamlanmıştır";
            }
            catch (DbUpdateException)
            {
                TempData["error"] = "Bir yada daha fazla kayıt ile ilişkili olduğundan silme işlemi yapılamıyor!";

            }
            return RedirectToAction(nameof(Index));
        }
    }
}
