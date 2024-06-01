using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    public class PostController : Controller
    {

        public readonly IWebHostEnvironment _env; 
        public PostController(IWebHostEnvironment env)
        {
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            var filePath = "";
            foreach (var item in Request.Form.Files)
            {
                string serverMapPath = Path.Combine(_env.WebRootPath, "ImageCK", item.FileName);
                using(var stream = new FileStream(serverMapPath, FileMode.Create))
                {
                    await item.CopyToAsync(stream);
                }
                filePath = "https://localhost:9002/" + "ImageCK/" + item.FileName; 
            }
            return Json(new { url = filePath });
        }
    }
}
