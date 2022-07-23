using BackendProject_Allup.DAL;
using BackendProject_Allup.Models;
using BackendProject_Allup.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BackendProject_Allup.ViewComponents
{
    public class HeadBasketViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;

        public HeadBasketViewComponent(AppDbContext context)    
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            string basket = Request.Cookies["basket"];

            List<BasketVM> products;

            if (basket != null)
            {
                products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);

                foreach (var item in products)
                {
                    Product product = _context.Products.Include(p=>p.ProductImages).FirstOrDefault(p => p.Id == item.Id);
                    item.Price = product.Price;
                    item.Name = product.Name;
                    item.ImgUrl = product.ProductImages.Find(p => p.IsMain==true).ImageUrl;
                }

            }
            else
            {
                products = new List<BasketVM>();
            }

            double total = 0;

            if (products.Count > 0)
            {
                foreach (BasketVM pr in products)
                {
                    total += pr.Price * pr.Count;
                    pr.SubTotal += pr.Price * pr.Count;
                    pr.BasketCount += pr.Count;
                }
            }
            ViewBag.Total = total;

            return View(await Task.FromResult(products));
        }
    }
}
