using BackendProject_Allup.DAL;
using BackendProject_Allup.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BackendProject_Allup.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public OrderController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
          
            return RedirectToAction("Checkout");
        }

        public IActionResult Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Order order = new Order();

            var basket = _context.Baskets.Include(b => b.BasketItems).ThenInclude(b => b.Product).FirstOrDefault(b => b.UserId == userId);

            ViewBag.BasketItems = basket.BasketItems.ToList();

            return View(order);
        }

        [HttpPost]
        public IActionResult Sale(Order order, string radio = "Cash")
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(String.Empty, "Error has been occured");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Order newOrder = new Order();

            var basket = _context.Baskets
                .Include(b => b.BasketItems)
                .ThenInclude(b => b.Product)
                .FirstOrDefault(b => b.UserId == userId);

            var basketItems = basket.BasketItems.ToList();


            newOrder.Address = order.Address;
            newOrder.City = order.City;
            newOrder.Country = order.Country;
            newOrder.CreatedAt = DateTime.Now;
            newOrder.Email = order.Email;
            newOrder.FirstName = order.FirstName;
            newOrder.Surname = order.Surname;
            newOrder.Phone = order.Phone;
            newOrder.UserId = userId;
            newOrder.OrderStatus = OrderStatus.New;
            newOrder.PaymantMethod = radio;


            _context.Orders.Add(newOrder);
            _context.SaveChanges();


            foreach (var item in basketItems)
            {
                OrderItem newOrderitem = new OrderItem();

                newOrderitem.ProductId = item.ProductId;
                newOrderitem.OrderId = newOrder.Id;
                newOrderitem.Count = item.Count;
                newOrderitem.Price = item.Product.Price;
                newOrderitem.Total += item.Product.Price * item.Count;
                newOrder.TotalPrice += newOrderitem.Total;

                _context.OrderItems.Add(newOrderitem);
                _context.BasketItems.Remove(item);
            }


            _context.SaveChanges();
            return RedirectToAction("index", "home");
        }

        public IActionResult OrderDetail(int? id)
        {
            if (id == null) return NotFound();

            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Product)
                .ThenInclude(o => o.ProductImages)
                .FirstOrDefault(o => o.Id == id);

            if(order == null) return NotFound();

            return View(order);
        }

    }
}
