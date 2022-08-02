
using BackendProject_Allup.DAL;
using BackendProject_Allup.Helpers;
using BackendProject_Allup.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendProject_Allup.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public OrderController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<IActionResult> Show(int page = 1, int pageSize = 10)
        {
            var orders = _context.Orders.ToList();

            var list = await PagedList<Order>.CreateAsync(orders, page, pageSize);

            return View(list);
        }

        public async Task<IActionResult> Orderstatus(OrderStatus orderStatus,int page = 1, int pageSize = 10)
        {
            var orders = _context.Orders.Where(x => x.OrderStatus == orderStatus).ToList();

            var list = await PagedList<Order>.CreateAsync(orders, page, pageSize);

            return View(list);
        }

        public async Task<IActionResult> Detail(int? orderid,string Returnurl)
        {
            if (orderid == null) return NotFound();

            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Product)
                .ThenInclude(o => o.ProductImages)
                .FirstOrDefault(o => o.Id == orderid);

            if (order == null) return NotFound();
           

            return View(order);
        }

        public IActionResult StatusControl(int id,string Returnurl, OrderStatus orderStatus)
        {
            Order order = _context.Orders.Find(id);

            switch (orderStatus)
            {
                case OrderStatus.Processing:
                    order.OrderStatus = OrderStatus.Processing;
                    break;
                case OrderStatus.Shipped:
                    order.OrderStatus = OrderStatus.Shipped;    
                    break;
                case OrderStatus.Completed:
                    order.OrderStatus = OrderStatus.Completed;
                    break;
                case OrderStatus.Closed:
                    order.OrderStatus = OrderStatus.Closed;
                    break;
                case OrderStatus.Canceled:
                    order.OrderStatus = OrderStatus.Canceled;
                    break;
            }

            _context.SaveChanges();
            EmailService emailService = new EmailService(_config.GetSection("ConfirmationParams:Email").Value, _config.GetSection("ConfirmationParams:Password").Value);
            emailService.SendEmail(order.Email, $"Order status {orderStatus.ToString()}", $"Your order received on {order.CreatedAt} is {orderStatus.ToString()}");


            if (Returnurl != null) return Redirect(Returnurl);

            return RedirectToAction("show");
        }

        private void SendInovoice(object ıd, string email, string v1, string v2, string v3)
        {
            throw new NotImplementedException();
        }
    }
}
