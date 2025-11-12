using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Checkout;
using TheHiddenSquirrel.Data;
using TheHiddenSquirrel.Models;

namespace TheHiddenSquirrel.Controllers
{
    public class ShopController : Controller
    {
        // shared db connection
        private readonly ApplicationDbContext _context;

        // config var to read Stripe Key from appsettings
        private readonly IConfiguration _configuration;

        // constructor to initialize db connection
        public ShopController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

        // POST: /Shop/AddToCart => add item to user's cart in db
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(int Quantity, int ProductId)
        {
            // look up product price 
            var product = _context.Product.Find(ProductId);
            var price = product.Price;

            // set id for cart using session var
            var customerId = GetCustomerId();

            // check if this product is already in customer's cart
            var cartItem = _context.CartItem
                .Where(c => c.ProductId == ProductId && c.CustomerId == customerId)
                .SingleOrDefault();  // get 1 record only not a list

            if (cartItem == null)
            {
                // create & save new cart item
                var newCartItem = new CartItem
                {
                    Quantity = Quantity,
                    ProductId = ProductId,
                    Price = price,
                    CustomerId = customerId
                };

                _context.CartItem.Add(newCartItem);
            }
            else
            {
                // update quantity on existing cart item
                cartItem.Quantity += Quantity;
                _context.CartItem.Update(cartItem);
            }
                _context.SaveChanges();

            // redirect to cart
            return RedirectToAction("Cart");
        }

        private string GetCustomerId()
        {
            // check for a CustomerId session var
            if (HttpContext.Session.GetString("CustomerId") == null)
            {
                // create & set CustomerId session var using GUID (Globally Unique Identifier)
                HttpContext.Session.SetString("CustomerId", Guid.NewGuid().ToString());
            }

            // send back the CustomerId string
            return HttpContext.Session.GetString("CustomerId");
        }

        // GET: /Shop/Cart => display user's full cart
        public IActionResult Cart()
        {
            // identify customer from session var
            var customerId = GetCustomerId();

            // fetch customer's cart items from db
            var cartItems = _context.CartItem
                .Where(c => c.CustomerId == customerId)
                .Include(c => c.Product)  // join to parent Product to also get Product Name
                .ToList();

            // show cart page w/data
            return View(cartItems);
        }

        // GET: /Shop/RemoveFromCart/28 => remove item from user's cart
        public IActionResult RemoveFromCart(int id)
        {
            // find and remove this cart item
            var cartItem = _context.CartItem.Find(id);
            _context.CartItem.Remove(cartItem);
            _context.SaveChanges();

            // refresh cart
            return RedirectToAction("Cart");
        }

        // GET: /Shop/Checkout => show blank form to capture customer details
        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }

        // POST: /Shop/Checkout => process Checkout form & save values to Session var before payment
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Checkout([Bind("FirstName,LastName,Address,City,Province,PostalCode,Phone")] Order order)
        {
            // auto-fill date, total, customerId
            order.OrderDate = DateTime.Now;
            order.CustomerId = User.Identity.Name;

            // calc order total
            order.OrderTotal = (from c in _context.CartItem
                                where c.CustomerId == GetCustomerId()
                                select c.Quantity * c.Price).Sum();

            // store order in Session var
            HttpContext.Session.SetObject("Order", order);
            return RedirectToAction("Payment");
        }

        // GET: /Shop/Payment => invoke Stripe Payment page
        [Authorize]
        public IActionResult Payment()
        {
            // get order from session var so we can read the OrderTotal
            var order = HttpContext.Session.GetObject<Order>("Order");
            var orderTotal = order.OrderTotal;

            // get Stripe API key from config
            StripeConfiguration.ApiKey = _configuration["StripeSecretKey"];

            // set up Stripe payment
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = new List<SessionLineItemOptions> { 
                    new SessionLineItemOptions
                    {
                        Quantity = 1,
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long?)(orderTotal * 100), // amount in cents
                            Currency = "cad",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "The Hidden Squirrel Purchase"
                            }
                        }
                    }
                },
                Mode = "payment",
                SuccessUrl = "https://" + Request.Host + "/Shop/SaveOrder",
                CancelUrl = "https://" + Request.Host + "/Shop/Cart"
            };

            // now invoke Stripe payment
            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);

            // after Payment, redirect based on success / cancel urls
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
    }
}
