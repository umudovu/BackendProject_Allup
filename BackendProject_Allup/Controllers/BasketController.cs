using BackendProject_Allup.DAL;
using BackendProject_Allup.Models;
using BackendProject_Allup.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BackendProject_Allup.Extentions;
using Microsoft.EntityFrameworkCore;

namespace BackendProject_Allup.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            List<BasketVM> products = BasketServiceExtentions.GetBasket(Request, _context);

            
            ViewBag.Total = BasketServiceExtentions.BasketCalculate(products);

            return View(products);
        }

        public IActionResult AddItem(int? id, string ReturnUrl)
        {
            if (id == null) return NoContent();

            string basket = Request.Cookies["basket"];

            Product product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product == null) return NoContent();

            List<BasketVM> products;

            if (basket == null)
            {
                products = new List<BasketVM>();
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            }

            BasketVM isexsist = products.Find(p => p.Id == id);

            if (isexsist == null)
            {
                BasketVM basketVM = new BasketVM
                {
                    Id = product.Id,
                    Count = 1,
                    Price = product.Price
                };
                products.Add(basketVM);
            }
            else
            {
                isexsist.Count++;
            }


            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products));

            

            if (products.Count > 0)
            {
                foreach (BasketVM pr in products)
                {
                    pr.SubTotal += pr.Price * pr.Count;
                    pr.BasketCount += pr.Count;
                }
            }
            if(ReturnUrl!=null) return Redirect(ReturnUrl);

            return RedirectToAction("index","shop");
        }

        public IActionResult Plus(int? id)
        {
            
            List<BasketVM> products = BasketServiceExtentions.GetBasket(Request,_context);

            BasketVM product = products.FirstOrDefault(p => p.Id == id);

            if (product == null) return NotFound();

            product.BasketCount++;

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products));

            
            return RedirectToAction("index");
        }

        public IActionResult Minus(int? id)
        {

            List<BasketVM> products = BasketServiceExtentions.GetBasket(Request, _context);

            BasketVM product = products.FirstOrDefault(p => p.Id == id);


            if (product == null) return NotFound();

            if (product.BasketCount > 0)
            {
                product.BasketCount--;

                Response.Cookies.Append("basket", JsonConvert.SerializeObject(products));
            }
            else
            {

                products.Remove(product);

                List<BasketVM> productsNew = products.FindAll(p => p.Id != id);

                Response.Cookies.Append("basket", JsonConvert.SerializeObject(productsNew));

            }

            return RedirectToAction("index");
        }
        public IActionResult Remove(int? id, string ReturnUrl)
        {
            

            List<BasketVM> products = BasketServiceExtentions.GetBasket(Request,_context);

            if (products == null) return NotFound();

            List<BasketVM> productsNew = products.FindAll(p => p.Id != id);

            BasketVM product = products.FirstOrDefault(p => p.Id == id);

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(productsNew));

            

            if (ReturnUrl != null) return Redirect(ReturnUrl);

            return RedirectToAction("index", "basket");
        }
    }
}
