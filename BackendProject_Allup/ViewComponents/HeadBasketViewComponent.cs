using BackendProject_Allup.DAL;
using BackendProject_Allup.Extentions;
using BackendProject_Allup.Models;
using BackendProject_Allup.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace BackendProject_Allup.ViewComponents
{
    public class HeadBasketViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HeadBasketViewComponent(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<BasketVM> products = new List<BasketVM>();
            var currentUserId = _userManager.GetUserId(Request.HttpContext.User);

            if(currentUserId!= null)
            {
                Basket basket = _context.Baskets
                    .Include(b => b.BasketItems)
                    .FirstOrDefault(b => b.UserId == currentUserId);

                List<BasketItem> basketItems = _context.BasketItems.Where(x => x.BasketId == basket.Id).ToList();
                foreach (var item in basketItems)
                {
                    Product product = _context.Products.Include(p => p.ProductImages).FirstOrDefault(p => p.Id == item.ProductId);

                    BasketVM basketVM = new BasketVM
                    {
                        Id = item.ProductId,
                        Price = product.Price,
                        Name = product.Name,
                        BasketCount = item.Count,
                        SubTotal = product.Price * item.Count,
                        ImgUrl = product.ProductImages.Find(p => p.IsMain == true).ImageUrl,

                    };
                    products.Add(basketVM);

                }


                ViewBag.Total = BasketServiceExtentions.BasketCalculate(products);
            }
            

            return View(await Task.FromResult(products));
        }

        //public async Task<IViewComponentResult> InvokeAsync()
        //{
        //    List<BasketVM> products= new List<BasketVM>();
        //    var currentUserId = _userManager.GetUserId(Request.HttpContext.User);


        //    Basket basket = _context.Baskets
        //            .Include(b => b.BasketItems)
        //            .FirstOrDefault(b => b.UserId == currentUserId);

        //    List<BasketItem> basketItems = _context.BasketItems.ToList();
        //    foreach (var item in basketItems)
        //    {
        //        Product product = _context.Products.Include(p => p.ProductImages).FirstOrDefault(p => p.Id == item.ProductId);

        //        BasketVM basketVM = new BasketVM
        //        {
        //            Id = item.ProductId,
        //            Price = product.Price,
        //            Name = product.Name,
        //            BasketCount = item.Count,
        //            SubTotal = product.Price * item.Count,
        //            ImgUrl = product.ProductImages.Find(p => p.IsMain == true).ImageUrl,

        //        };
        //        products.Add(basketVM);

        //    }


        //    ViewBag.Total = BasketServiceExtentions.BasketCalculate(products);

        //    return View(await Task.FromResult(products));
        //}
    }
}
