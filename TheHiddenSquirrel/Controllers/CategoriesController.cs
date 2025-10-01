using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
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

        // GET: /Categories/Edit/5 => show form populated with current Name for selected Category
        public IActionResult Edit(int id)
        {
            // search db for Category with this id
            var category = _context.Category.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            // display populated form to edit a category
            return View(category);
        }

        // POST: /Categories/Edit/5 => process update of selected Category in the db
        [HttpPost]
        public IActionResult Edit(int id, [Bind("CategoryId,Name")] Category category)
        {
            // validate input
            if (!ModelState.IsValid)
            {
                return View();
            }

            // update db
            _context.Category.Update(category);
            _context.SaveChanges();

            // redirect to list on Index
            return RedirectToAction("Index");
        }

        // GET: /Categories/Delete/2 => delete selected Category from db
        public IActionResult Delete(int id)
        {
            // get record to be deleted
            var category = _context.Category.Find(id);

            // id not found handler
            if (category == null)
            {
                return NotFound();
            }

            // remove & redirect
            _context.Category.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
