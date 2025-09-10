using Microsoft.AspNetCore.Mvc;

namespace TheHiddenSquirrel.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
