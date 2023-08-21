using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
