using Microsoft.AspNetCore.Mvc;
using TheHiddenSquirrel.Data;
using TheHiddenSquirrel.Models;

namespace TheHiddenSquirrel.Controllers
{
    public class CategoriesController : Controller
    {
        // shared db connection obj
        private readonly ApplicationDbContext _context;

        // constructor with connection dependency
        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // use Category model to create list of in-memory categories (replaced by db soon)
            //var categories = new List<Category>();

            //for (var i = 1; i < 11; i++)
            //{
            //    categories.Add(new Category { CategoryId = i, Name = "Category " + i.ToString() });
            //}

            // use DbSet to fetch list of categories from db
            var categories = _context.Category.ToList();

            // pass categories list for display when loading the view
            return View(categories);
        }

        public IActionResult Create()
        {
            // display blank form to add a new category
            return View();
        }

        // POST: /Categories/Create => process form submission to save new Category
        [HttpPost]
        public IActionResult Create([Bind("Name")] Category category)
        {
            // validate category first
            if (!ModelState.IsValid)
            {
                return View();
            }

            // save to db
            _context.Category.Add(category);
            _context.SaveChanges();

            // redirect to Index page
            return RedirectToAction("Index");
        }

        public IActionResult Edit()
        {
            // display populated form to edit a category
            return View();
        }
    }
}
