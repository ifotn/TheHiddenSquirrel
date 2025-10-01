using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheHiddenSquirrel.Data;
using TheHiddenSquirrel.Models;

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
            // fetch Products
            var products = _context.Product.ToList();

            // show view & display Product data
            return View(products);
        }

        // GET: /Products/Create => show empty Product form including Category dropdown
        public IActionResult Create()
        {
            // fetch list of Categories for Category dropdown in the form
            ViewBag.CategoryId = new SelectList(_context.Category.ToList(), "CategoryId", "Name");

            // display blank form to add a new product
            return View();
        }

        // POST: /Products/Create => validate & save new Product
        [HttpPost]
        public IActionResult Create([Bind("Name,Description,Price,Age,Image,Rating,CategoryId")] Product product)
        {
            // validate inputs
            if (!ModelState.IsValid)
            {
                // fetch list of Categories for Category dropdown in the form
                ViewBag.CategoryId = new SelectList(_context.Category.ToList(), "CategoryId", "Name");
                return View(product);
            }

            // save to db & redirect
            _context.Product.Add(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit()
        {
            // display populated form to edit a product
            return View();
        }
    }
}
