using Microsoft.AspNetCore.Mvc;
using BackendProject_Allup.DAL;
using BackendProject_Allup.Models;
using Microsoft.AspNetCore.Identity;
using BackendProject_Allup.ViewModels;
using System.Security.Claims;

namespace BackendProject_Allup.Controllers
{
    public class MyAccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public MyAccountController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("login", "account");

            var orders = _context.Orders.Where(o => o.UserId == userId).ToList();

            MyAccountVM myAccountVM = new MyAccountVM()
            {
                Orders = orders
            };
            return View(myAccountVM);
        }

        [HttpPost]
        public async Task<IActionResult> Payment(PaymentVM payment)
            {
            if (!ModelState.IsValid) return RedirectToAction("index");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(x=>x.Id==userId);

            if (user == null) return NotFound();

            user.Balance += payment.Amount;

            _context.SaveChanges();


            return RedirectToAction("index");
        }
    }
}
