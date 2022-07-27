using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using BackendProject_Allup.ViewModels;
using Newtonsoft.Json;
using BackendProject_Allup.Models;
using BackendProject_Allup.DAL;
using Microsoft.EntityFrameworkCore;

namespace BackendProject_Allup.Extentions
{
    public static class BasketServiceExtentions
    {
        public static double BasketCalculate(List<BasketVM> products)
        {
            double total = 0;

            if (products.Count > 0)
            {
                foreach (var pr in products)
                {
                    total += pr.Price * pr.BasketCount;
                }
            }
            return total;
        }


        public static List<BasketVM> GetBasket(HttpRequest request, AppDbContext context)
        {
            string basket = request.Cookies["basket"];
            List<BasketVM> products;

            if (basket != null)
            {
                products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);

                foreach (var item in products)
                {
                    Product product = context.Products.Include(p => p.ProductImages).FirstOrDefault(p => p.Id == item.Id);
                    item.Price = product.Price;
                    item.Name = product.Name;
                    item.ImgUrl = product.ProductImages.Find(p => p.IsMain == true).ImageUrl;
                }

            }
            else
            {
                products = new List<BasketVM>();
            }
            return products;
        }
    }
}
