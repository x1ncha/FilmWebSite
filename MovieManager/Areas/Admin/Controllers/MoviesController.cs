using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieManager.Data;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using X.PagedList;

namespace MovieManager.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MoviesController : Controller
    {
        private readonly AppDbContext context;

        public MoviesController(AppDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index(int? page)
        {
            var model = context.Movies.OrderBy(p => p.Name).ToPagedList(page ?? 1, 10);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Genres = new SelectList(await context.Genres.OrderBy(p => p.Name).ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Movie model)
        {
            if (model.ImageFile is not null)
            {
                using var image = await Image.LoadAsync(model.ImageFile.OpenReadStream());
                //using var ms = new MemoryStream();
                //BlobClient blobClient = new BlobClient("sdvsv", "Container1", "Films");

                image.Mutate(p => p.Resize(new ResizeOptions
                {
                    Size = new Size(500, 740),
                    Mode = ResizeMode.Crop
                }));
                //image.SaveAsJpeg(ms);
                //var response = await blobClient.UploadAsync(ms);
                //model.Image = response.Value.BlobSequenceNumber.ToString();
                model.Image = image.ToBase64String(JpegFormat.Instance);

            }

            context.Movies.Add(model);
            context.SaveChanges();
            TempData["success"] = "Film ekleme işlemi başarıyla tamamlanmıştır";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Genres = new SelectList(await context.Genres.OrderBy(p => p.Name).ToListAsync(), "Id", "Name");
            var model = context.Movies.Find(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Movie model)
        {
            if (model.ImageFile is not null)
            {
                using var image = await Image.LoadAsync(model.ImageFile.OpenReadStream());
                //using var ms = new MemoryStream();
                //BlobClient blobClient = new BlobClient("sdvsv", "Container1", "Films");

                image.Mutate(p => p.Resize(new ResizeOptions
                {
                    Size = new Size(500, 740),
                    Mode = ResizeMode.Crop
                }));
                //image.SaveAsJpeg(ms);
                //var response = await blobClient.UploadAsync(ms);
                //model.Image = response.Value.BlobSequenceNumber.ToString();
                model.Image = image.ToBase64String(JpegFormat.Instance);

            }
            context.Movies.Update(model);
            context.SaveChanges();
            TempData["success"] = "Film güncelleme işlemi başarıyla tamamlanmıştır";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var model = context.Movies.Find(id);
            context.Movies.Remove(model);
            context.SaveChanges();
            TempData["success"] = "Film silme işlemi başarıyla tamamlanmıştır";
            return RedirectToAction(nameof(Index));
        }
    }
}
