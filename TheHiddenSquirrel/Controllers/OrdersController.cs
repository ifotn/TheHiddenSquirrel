using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheHiddenSquirrel.Data;
using TheHiddenSquirrel.Models;

namespace TheHiddenSquirrel.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        // db conn
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Orders => show list of orders newest to oldest
        public IActionResult Index()
        {
            // fetch order list
            var orders = new List<Order>();

            if (User.IsInRole("Administrator"))
            {
                // fetch all orders for all customers
                orders = _context.Order.OrderByDescending(o => o.OrderDate).ToList();
            }
            else
            {
                // fetch all orders only for the current authenticated user
                orders = _context.Order
                    .Where(o => o.CustomerId == User.Identity.Name)
                    .OrderByDescending(o => o.OrderDate).ToList();
            }

            return View(orders);
        }

        // GET: /Orders/Details/24 => show Order Confirmation w/all details
        public IActionResult Details(int id)
        {
            var order = new Order();

            if (User.IsInRole("Administrator"))
            {
                order = _context.Order
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .SingleOrDefault(o => o.OrderId == id);
            }
            else
            {
                // fetch order from db if User is Customer (can't fetch orders by others)
                order = _context.Order
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .Where(o => o.CustomerId == User.Identity.Name)
                    .SingleOrDefault(o => o.OrderId == id);
            }

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
