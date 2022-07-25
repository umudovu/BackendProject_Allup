using BackendProject_Allup.DAL;
using BackendProject_Allup.Models;
using BackendProject_Allup.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BackendProject_Allup.ViewComponents
{
    public class HeaderViewComponent: ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public HeaderViewComponent(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            HomeVM home = new HomeVM();
            home.Categories = _context.Categories.ToList();


            home.Username = "";

            ViewBag.Category = home.Categories;

            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                home.Username = user.FullName.Split(" ")[0];
            }
            home.Bio = _context.Bios.FirstOrDefault();


            return View(await Task.FromResult(home));
        }
    }
}
