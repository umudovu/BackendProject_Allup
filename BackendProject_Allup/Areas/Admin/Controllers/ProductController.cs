using BackendProject_Allup.Areas.Admin.ViewModels;
using BackendProject_Allup.DAL;
using BackendProject_Allup.Extentions;
using BackendProject_Allup.Helpers;
using BackendProject_Allup.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BackendProject_Allup.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Show(int page = 1, int pageSize = 10)
        {
            List<Product> products = await _context.Products
                .Include(x => x.ProductImages)
                .Include(x => x.Brand)
                .Include(x => x.TagProducts)
                .Include(x => x.Category)
                .ToListAsync();


            var list = await PagedList<Product>.CreateAsync(products, page, pageSize);

            return View(list);
        }

        public async Task<IActionResult> Create()
        {

            ProductVM productVM = new ProductVM();
            ViewBag.Categories = new SelectList(_context.Categories.Where(x => x.ParentId != null).ToList(), "Id", "Name");
            ViewBag.Brands = new SelectList(_context.Brands.ToList(), "Id", "Name");

            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductVM productVM, string Returnurl)
        {
            if (ModelState["Name"] == null)
            {
                ModelState.AddModelError("Name", "Name is null here");
                return View();
            }
            if (ModelState["Photos"] == null)
            {
                ModelState.AddModelError("Photo", "Photo is null here");
                return View();
            }

            foreach (var file in productVM.Photos)
            {
                if (!file.IsImage())
                {
                    ModelState.AddModelError("Photo", "Only image");
                    return View();
                }
                if (file.ImageSize(800))
                {
                    ModelState.AddModelError("Photo", "Olcu oversize");
                    return View();
                }
            }

            if (_context.Products.Any(x => x.Name.ToLower() == productVM.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Eyni ad is exsist");
                return View();
            }

            Product newProduct = new Product()
            {
                Name = productVM.Name,
                Description = productVM.Description,
                Price = productVM.Price,
                DiscountPrice = productVM.DiscountPrice,
                TaxPercent = productVM.TaxPercent,
                StockCount = productVM.StockCount,
                CategoryId = productVM.CategoryId,
                BrandId = productVM.BrandId,
                IsFeatured = productVM.IsFeatured,
                BestSeller = productVM.BestSeller,
                NewArrival = productVM.NewArrival,
                CreatedAt = DateTime.Now
            };

            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            List<ProductImage> productImages = new List<ProductImage>();

            foreach (var photo in productVM.Photos)
            {
                ProductImage productImage = new ProductImage();

                if (productVM.Photos[0] == photo) productImage.IsMain = true; else productImage.IsMain = false;

                productImage.ImageUrl = "images/product/" + photo.SaveImage(_env, @"assets\images\product");
                productImage.ProductId = newProduct.Id;
                productImage.CreatedAt = DateTime.Now;

                await _context.ProductImages.AddAsync(productImage);
                newProduct.ProductImages.Add(productImage);
            }
            
            await _context.SaveChangesAsync();

            return Redirect(Returnurl);
        }


    }
}
