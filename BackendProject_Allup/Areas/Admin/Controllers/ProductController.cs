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
                if (!photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Only image");
                    return View();
                }
                if (photo.ImageSize(800))
                {
                    ModelState.AddModelError("Photo", "Olcu oversize");
                    return View();
                }
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


        public async Task<IActionResult> Update(int?id)
        {
            if (id == null) return NotFound();
            var product = await _context.Products.Include(x=>x.ProductImages).FirstOrDefaultAsync(x=>x.Id==id);
            if(product == null) return NotFound();

            ProductVM productVM = new ProductVM()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DiscountPrice = product.DiscountPrice,
                TaxPercent = product.TaxPercent,
                StockCount = product.StockCount,
                CategoryId = product.CategoryId,
                BrandId = product.BrandId,
                IsFeatured = product.IsFeatured,
                BestSeller = product.BestSeller,
                NewArrival = product.NewArrival,
                Images = product.ProductImages
            };

            ViewBag.Categories = new SelectList(_context.Categories.Where(x => x.ParentId != null).ToList(), "Id", "Name");
            ViewBag.Brands = new SelectList(_context.Brands.ToList(), "Id", "Name");

            return View(productVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductVM productVM, string Returnurl)
        {
            Product dbProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == productVM.Id);
            if (dbProduct == null) return NotFound();

            if (productVM.Photos != null)
            {
                foreach (var photo in productVM.Photos)
                {
                    if (!photo.IsImage())
                    {
                        ModelState.AddModelError("Photo", "Only image");
                        return View();
                    }
                    if (photo.ImageSize(800))
                    {
                        ModelState.AddModelError("Photo", "Olcu oversize");
                        return View();
                    }

                    ProductImage productImage = new ProductImage();


                    productImage.ImageUrl = "images/product/" + photo.SaveImage(_env, @"assets\images\product");
                    productImage.ProductId = dbProduct.Id;
                    productImage.CreatedAt = DateTime.Now;

                    await _context.ProductImages.AddAsync(productImage);
                    dbProduct.ProductImages.Add(productImage);
                }
            }

            var productName = _context.Products.FirstOrDefault(x => x.Name.ToLower() == productVM.Name.ToLower());
            if (productName != null)
            {
                if (dbProduct.Name.ToLower() != productName.Name.ToLower())
                {
                    ModelState.AddModelError("Name", "Model Name is exsist");
                    return View("Update");
                }
            }

            dbProduct.Name = productVM.Name;
            dbProduct.Description = productVM.Description;
            dbProduct.Price = productVM.Price;
            dbProduct.DiscountPrice = productVM.DiscountPrice;
            dbProduct.TaxPercent = productVM.TaxPercent;
            dbProduct.StockCount = productVM.StockCount;
            dbProduct.CategoryId = productVM.CategoryId;
            dbProduct.BrandId = productVM.BrandId;
            dbProduct.IsFeatured = productVM.IsFeatured;
            dbProduct.BestSeller = productVM.BestSeller;
            dbProduct.NewArrival = productVM.NewArrival;
            dbProduct.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();


           if(Returnurl!=null) return Redirect(Returnurl);
            return RedirectToAction("show");
        }
        public async Task<IActionResult> MainImage(int? imageid, int? productid, string Returnurl)
        {
            if (imageid == null || productid == null) return NotFound();
            var image = await _context.ProductImages.FirstOrDefaultAsync(x => x.Id == imageid && x.ProductId == productid);
            if (image == null) return NotFound();

            var product =await _context.Products.Include(x=>x.ProductImages).FirstOrDefaultAsync(x=>x.Id==productid);
            var mainImage = product.ProductImages.FirstOrDefault(x => x.IsMain);
            mainImage.IsMain = false;

            image.IsMain = true;
            await _context.SaveChangesAsync();

            return Redirect(Returnurl);
        }
        public async Task<IActionResult> RemoveImage(int? imageid,int? productid, string Returnurl)
        {
            if (imageid == null || productid == null) return NotFound();
            var image = await _context.ProductImages.FirstOrDefaultAsync(x => x.Id == imageid && x.ProductId == productid);
            if (image == null) return NotFound();

            //var isMain = await _context.ProductImages.FirstOrDefaultAsync(x => x.Id == imageid && x.ProductId == productid);
            //if (isMain.IsMain) return View();

            string path = Path.Combine(_env.WebRootPath, @"assets\images\product", image.ImageUrl);
            Helpers.Helpers.DeleteImage(path);

            _context.ProductImages.Remove(image);
            await _context.SaveChangesAsync();

            return Redirect(Returnurl);
        }

        public async Task<IActionResult> Remove(int?id, string Returnurl)
        {
            Product dbProduct = await _context.Products
                .Include(x=>x.ProductImages)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (dbProduct == null) return NotFound();

            foreach (var photo in dbProduct.ProductImages)
            {
                string path = Path.Combine(_env.WebRootPath, @"assets\images\product", photo.ImageUrl);
                Helpers.Helpers.DeleteImage(path);

                _context.ProductImages.Remove(photo);
            }

            _context.Products.Remove(dbProduct);

            await _context.SaveChangesAsync();


            if (Returnurl != null) return Redirect(Returnurl);
            return RedirectToAction("show");
        }

    }
}
