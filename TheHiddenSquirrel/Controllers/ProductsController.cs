using Microsoft.AspNetCore.Mvc;

namespace TheHiddenSquirrel.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            // display blank form to add a new product
            return View();
        }
    }
}
