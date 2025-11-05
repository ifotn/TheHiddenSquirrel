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

        // GET: /Shop/Products/32 => show Products from selected Category
        public IActionResult Products(int id)
        {
            // fetch products ONLY for selected category
            var products = _context.Product.Where(p => p.CategoryId == id)
                .OrderBy(p => p.Name)
                .ToList();

            // fetch category name for page heading & title
            var category = _context.Category.Find(id);
            ViewData["Category"] = category.Name;

            // show view and pass query result
            return View(products);
        }

        // GET: /Shop/ProductDetails/27 => show Product Detail 
        public IActionResult ProductDetails(int id)
        {
            // fetch selected product
            var product = _context.Product.Find(id);

            // error handling if id doesn't exist
            if (product == null)
            {
                return NotFound();
            }

            // pass to view for display
            return View(product);
        }
    }
}
