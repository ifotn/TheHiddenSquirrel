using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheHiddenSquirrel.Data;

namespace TheHiddenSquirrel.Controllers
{
    public class ProductsController : Controller
    {
        // shared dbcontext obj
        private readonly ApplicationDbContext _context;

        // constructor to instantiate dbcontext connection
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            // fetch list of Categories for Category dropdown in the form
            ViewBag.CategoryId = new SelectList(_context.Category.ToList(), "CategoryId", "Name");

            // display blank form to add a new product
            return View();
        }

        public IActionResult Edit()
        {
            // display populated form to edit a product
            return View();
        }
    }
}
