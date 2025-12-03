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

        // PUT: /api/v1/products/3 => updated selected product
        [HttpPut("{id}")]
        public IActionResult Update(int id, Product product)
        {
            // validate for model errors
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // valid id in uri matches id of product object
            if (id != product.ProductId)
            {
                return BadRequest("Invalid ProductId");
            }

            // update
            _context.Product.Update(product);
            _context.SaveChanges();
            return Ok();
        }

        // DELETE: /api/v1/products/3 => delete selected product
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _context.Product.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            // delete
            _context.Product.Remove(product);
            _context.SaveChanges();
            return Ok();
        }

    }
}
