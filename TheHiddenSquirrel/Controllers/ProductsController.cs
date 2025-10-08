using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheHiddenSquirrel.Data;
using TheHiddenSquirrel.Models;

namespace TheHiddenSquirrel.Controllers
{
    [Authorize(Roles = "Administrator")]  // restrict all methods to Administrator role only
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
            // fetch Products, JOIN to Category model to include Category Name values, sorted a-z by Product Name
            // "p" means the result of the Product DbSet query
            var products = _context.Product
                .OrderBy(p => p.Name)
                .Include(p => p.Category)
                .ToList();

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
        [ValidateAntiForgeryToken]  // protect from cross-site scripting attacks
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

        // GET: /Products/Edit/6 => display form with Product data populated
        public IActionResult Edit(int id) 
        {
            var product = _context.Product.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            // fetch list of Categories for Category dropdown in the form
            ViewBag.CategoryId = new SelectList(_context.Category.ToList(), "CategoryId", "Name");

            // display populated form to edit a product
            return View(product);
        }

        // POST: /Products/Edit/6 => save Product updates to db
        [HttpPost]
        [ValidateAntiForgeryToken]  // protect from cross-site scripting attacks
        public IActionResult Edit(int id, [Bind("ProductId,Name,Description,Price,Age,Image,Rating,CategoryId")] Product product)
        {
            // validate
            if (!ModelState.IsValid)
            {
                // fetch list of Categories for Category dropdown in the form
                ViewBag.CategoryId = new SelectList(_context.Category.ToList(), "CategoryId", "Name");

                // show Edit page again w/current values still in it
                return View(product);
            }

            // save & redirect
            _context.Product.Update(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Products/Delete/3 => remove Product and refresh list
        public IActionResult Delete(int id)
        {
            var product = _context.Product.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            // delete & redirect
            _context.Product.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
