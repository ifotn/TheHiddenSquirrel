using Microsoft.AspNetCore.Mvc;
using TheHiddenSquirrel.Data;

namespace TheHiddenSquirrel.Controllers
{
    public class ShopController : Controller
    {
        // shared db connection
        private readonly ApplicationDbContext _context;

        // constructor to initialize db connection
        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Shop/Index => show Categories as cards so user can select one
        public IActionResult Index()
        {
            // fetch categories
            var categories = _context.Category.OrderBy(c => c.Name).ToList();

            // show view and pass categories list
            return View(categories);
        }
    }
}
