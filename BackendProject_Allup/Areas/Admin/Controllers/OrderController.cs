
using BackendProject_Allup.DAL;
using BackendProject_Allup.Helpers;
using BackendProject_Allup.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendProject_Allup.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        private readonly AppDbContext _context;
        public OrderController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Show(int page = 1, int pageSize = 10)
        {
            var orders = _context.Orders.ToList();

            var list = await PagedList<Order>.CreateAsync(orders, page, pageSize);

            return View(list);
        }

        public IActionResult StatusControl(int id,string Returnurl, string status = "New")
        {
            Order order = _context.Orders.Find(id);


            switch (status)
            {
                case "New":
                    order.OrderStatus = OrderStatus.New;
                    break;
                case "Delivering":
                    order.OrderStatus = OrderStatus.Delivering;
                    break;
                case "Completed":
                    order.OrderStatus = OrderStatus.Completed;
                    break;
                case "Closed":
                    order.OrderStatus = OrderStatus.Closed;
                    break;
                case "Canceled":
                    order.OrderStatus = OrderStatus.Canceled;
                    break;
            }


            _context.SaveChanges();

            if (Returnurl != null) return Redirect(Returnurl);

            return RedirectToAction("show");
        }
    }
}
