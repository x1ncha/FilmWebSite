using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieManager.Areas.Admin.Models;
using QRCoder;
using System.Drawing;
using X.PagedList;
using static QRCoder.PayloadGenerator;

namespace MovieManager.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrators,ProductAdministrators,OrderAdministrators")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext context;

        public DashboardController(
            AppDbContext context
            )
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var model = new DashboardViewModel
            {
                Genres = context.Genres.Count(),
                Movies = context.Movies.Count(),
                Users = context.Users.Count(),
                Comments = context.Comments.Count(),
                Top3Movies = context.Movies
                    .OrderByDescending(m => m.Views)
                    .Take(3)
                    .ToList()
            };

           

            return View(model);
        }

        public IActionResult usersdetail(int? page)
        {
            var model = context.AppUsers.OrderBy(p => p.FirstName).ToPagedList(page ?? 1, 10);
            return View(model);
        }

        public IActionResult GetQrCode(string id)
        {
            var generator = new QRCodeGenerator();

            var codeData = generator.CreateQrCode("https://www.linkedin.com/in/yiğit-can-721b85230/" + id, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(codeData);
            return File(qrCode.GetGraphic(20), "image/png");
        }
    }
}
