using BackendProject_Allup.Areas.Admin.ViewModels;
using BackendProject_Allup.DAL;
using BackendProject_Allup.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendProject_Allup.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public DashboardController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.UserName = "";

            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.UserName = user.UserName;
            }
            DashboardVM dashboard = new()
            {
                Products = await _context.Products
                    .Where(x => x.IsDeleted == false)
                    .Include(x => x.ProductImages)
                    .Include(x => x.Brand)
                    .Include(x => x.TagProducts)
                    .Include(x => x.Category)
                    .ToListAsync(),
                Users = await _context.Users.ToListAsync(),
                OrderTotalprice = _context.Orders.Sum(x => x.TotalPrice)
            };

            return View(dashboard);
        }
    }
}
