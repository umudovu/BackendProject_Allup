using BackendProject_Allup.DAL;
using BackendProject_Allup.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendProject_Allup.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;

        public FooterViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int take)
        {

            Bio bio = _context.Bios.FirstOrDefault();

            return View(await Task.FromResult(bio));
        }
    }
}
