using BackendProject_Allup.DAL;
using BackendProject_Allup.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendProject_Allup.Areas.Admin.Controllers
{
    public class BrandController : BaseController
    {
        private readonly AppDbContext _context;

        public BrandController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List <Brand> brands= _context.Brands.ToList();
            return View(brands);
        }
    }
}
