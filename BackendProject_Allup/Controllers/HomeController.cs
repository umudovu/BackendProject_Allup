using BackendProject_Allup.DAL;
using BackendProject_Allup.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BackendProject_Allup.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            HomeVM home = new HomeVM();
            home.Sliders = _context.Sliders.ToList();
            home.Categories=_context.Categories.ToList();
            home.Products = _context.Products
                    .Include(p => p.ProductImages)
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .ToList();
            home.Brands=_context.Brands.ToList();


            return View(home);
        }
        public IActionResult Detail(int? id)
        {
            if (id == null) return NotFound();
            var product = _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);

            if (product == null) return NotFound();

            return PartialView("_PartialProductModal", product);
        }

    }
}