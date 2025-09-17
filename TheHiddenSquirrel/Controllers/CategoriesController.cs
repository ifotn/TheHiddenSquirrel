using Microsoft.AspNetCore.Mvc;
using TheHiddenSquirrel.Models;

namespace TheHiddenSquirrel.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            // use Category model to create list of in-memory categories (replaced by db soon)
            var categories = new List<Category>();

            for (var i = 1; i < 11; i++)
            {
                categories.Add(new Category { CategoryId = i, Name = "Category " + i.ToString() });
            }

            // pass categories list for display when loading the view
            return View(categories);
        }

        public IActionResult Create()
        {
            // display blank form to add a new category
            return View();
        }

        public IActionResult Edit()
        {
            // display populated form to edit a category
            return View();
        }
    }
}
