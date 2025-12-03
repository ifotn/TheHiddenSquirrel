using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheHiddenSquirrel.Data;
using TheHiddenSquirrel.Models;

namespace TheHiddenSquirrel.Controllers.api.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // shared db conn for all CRUD methods
        private readonly ApplicationDbContext _context;

        // constructor w/db dependency
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /api/v1/products => show all products
        [HttpGet]
        public IActionResult Index()
        {
            // return all products as json
            var products = _context.Product.ToList();

            return Ok(products);
        }

        // GET: /api/v1/products/28 => show selected product
        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            // get selected product
            var product = _context.Product.Find(id);

            // if invalid id => 404
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: /api/v1/products => insert new product from request body
        [HttpPost]
        public IActionResult Create(Product product)
        {
            // validate for model errors
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // insert to db
            _context.Product.Add(product);
            _context.SaveChanges();
            return CreatedAtAction("Create", product);
        }

    }
}
