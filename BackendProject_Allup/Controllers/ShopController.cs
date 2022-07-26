using BackendProject_Allup.DAL;
using BackendProject_Allup.Helpers;
using BackendProject_Allup.Models;
using BackendProject_Allup.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendProject_Allup.Controllers
{
    public class ShopController : Controller
    {

        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ShopController(AppDbContext context, Microsoft.AspNetCore.Identity.UserManager<AppUser> userManager = null)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
        {
            ShopVM shopVM = new ShopVM()
            {
                Categories = _context.Categories.ToList(),
                Products = _context.Products
                    .Include(p => p.ProductImages)
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .ToList()
            };

            var list = await PagedList<Product>.CreateAsync(shopVM.Products, page, pageSize);


            return View(list);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product dbProcduct = _context.Products
                    .Include(p => p.ProductImages)
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .Include(p => p.TagProducts)
                    .FirstOrDefault(p => p.Id == id);

            if (dbProcduct == null) return NotFound();
            ShopVM shopVM = new ShopVM();

            shopVM.Product = dbProcduct;
            shopVM.Categories = _context.Categories.ToList();
            shopVM.Products=_context.Products.Include(p=>p.ProductImages).ToList();
            shopVM.Brands=_context.Brands.ToList();
            shopVM.Reviews = _context.Reviews.ToList();
            shopVM.Comments = _context.Comments.Include(c=>c.User).Where(c => c.ProductId == dbProcduct.Id).ToList();


            return View(shopVM);
        }


        [HttpPost]
        public async Task<IActionResult> AddComment(int productId, string content)
        {
            if (content==null) return View();

            AppUser user = new AppUser();
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            else
            {
                return RedirectToAction("login", "account");
            }

            Comment newComment = new Comment
            {
                Content = content,
                UserId = user.Id,
                ProductId = productId,
                CreatedAt = DateTime.Now
            };

            await _context.AddAsync(newComment);
            _context.SaveChanges();

            return RedirectToAction("detail", new { id = productId });
        }

    }
}
