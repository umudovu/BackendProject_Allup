using BackendProject_Allup.DAL;
using BackendProject_Allup.Extentions;
using BackendProject_Allup.Models;
using BackendProject_Allup.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackendProject_Allup.ViewComponents
{
    public class HeaderOrderViewComponent: ViewComponent
    {

        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HeaderOrderViewComponent(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var userId = _userManager.GetUserId(Request.HttpContext.User);
            var orders = _context.Orders.Where(o => o.UserId == userId).Where(x=>x.OrderStatus!= OrderStatus.Completed).ToList();

            return View(await Task.FromResult(orders));
        }
    }
}
