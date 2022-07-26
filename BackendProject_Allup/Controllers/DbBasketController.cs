using BackendProject_Allup.DAL;
using BackendProject_Allup.Extentions;
using BackendProject_Allup.Models;
using BackendProject_Allup.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BackendProject_Allup.Controllers
{
    public class DbBasketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DbBasketController(AppDbContext context, UserManager<AppUser> userManager = null)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null) return RedirectToAction("login", "account");

            Basket basket = _context.Baskets
                    .Include(b => b.BasketItems)
                    .FirstOrDefault(b => b.UserId == currentUserId);


            if (basket == null)
            {
                return RedirectToAction("index", "shop");
            }


            List<BasketItem> basketItems = _context.BasketItems.ToList();

            List<BasketVM> products = new List<BasketVM>();

            foreach (var item in basketItems)
            {
                Product product = _context.Products.Include(p => p.ProductImages).FirstOrDefault(p => p.Id == item.ProductId);

                BasketVM basketVM = new BasketVM
                {
                    Price = product.Price,
                    Name = product.Name,
                    BasketCount = item.Count,
                    SubTotal = product.Price * item.Count,
                    ImgUrl = product.ProductImages.Find(p => p.IsMain == true).ImageUrl
                };
            products.Add(basketVM);

        }


        ViewBag.Total = BasketServiceExtentions.BasketCalculate(products);

            return View(products);
    }


    public IActionResult AddItem(int? id, string ReturnUrl)
    {

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (currentUserId == null) return RedirectToAction("login", "account");
        Basket basket = _context.Baskets
                .Include(b => b.BasketItems)
                .FirstOrDefault(b => b.UserId == currentUserId);

        if (id == null) return NoContent();

        Product product = _context.Products.FirstOrDefault(x => x.Id == id);
        if (product == null) return NoContent();


        List<BasketItem> basketItems = _context.BasketItems.Where(b => b.BasketId == basket.Id).ToList(); ;

        BasketItem isexsist = basketItems.Find(p => p.ProductId == id);

        if (isexsist == null)
        {
            BasketItem basketItem = new BasketItem();

            basketItem.ProductId = product.Id;
            basketItem.Count = 1;
            basketItem.BasketId = basket.Id;

            _context.Add(basketItem);
        }
        else
        {
            isexsist.Count++;
        }

        _context.SaveChanges();


        return RedirectToAction("index", "shop");
    }

}
}
