using BackendProject_Allup.Areas.Admin.ViewModels;
using BackendProject_Allup.DAL;
using BackendProject_Allup.Extentions;
using BackendProject_Allup.Helpers;
using BackendProject_Allup.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendProject_Allup.Areas.Admin.Controllers
{
    public class BrandController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BrandController(AppDbContext context, IWebHostEnvironment env = null)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Show(int page = 1, int pageSize = 10)
        {
            List<Brand> brands = _context.Brands.Where(x=>x.IsDeleted==false).ToList();

            var list = await PagedList<Brand>.CreateAsync(brands, page, pageSize);

            return View(list);
        }

        public IActionResult Create()
        {
            BrandVM brandVM = new BrandVM();
            return View(brandVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandVM brand)
        {
            if (ModelState["Photo"] == null)
            {
                ModelState.AddModelError("Photo", "Photo is null");
                return View();
            }
            if (!brand.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Only image");
                return View();
            }
            if (brand.Photo.ImageSize(800))
            {
                ModelState.AddModelError("Photo", "Olcu oversize");
                return View();
            }

            if (_context.Brands.Any(x=>x.Name.ToLower()==brand.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Eyni ad is exsist");
                return View();
            }

            Brand newBrand = new Brand()
            {
                Name = brand.Name,
                ImageUrl = "images/brand/" + brand.Photo.SaveImage(_env, @"assets\images\brand"),
                CreatedAt = DateTime.Now
            };

            await _context.Brands.AddAsync(newBrand);
            await _context.SaveChangesAsync();

            return RedirectToAction("show");
        }

        public IActionResult Update(int?id)
        {
            if (id == null) return NotFound();

            Brand dbBrand = _context.Brands.FirstOrDefault(b => b.Id == id);
            if(dbBrand == null) return NotFound();

            BrandVM brand = new BrandVM()
            {
                Id=dbBrand.Id,
                Name = dbBrand.Name,
            };

            return View(brand);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(BrandVM brand)
		{
            Brand dbBrand = await _context.Brands.FirstOrDefaultAsync(x => x.Id == brand.Id);
            if (dbBrand == null) return NotFound();

            if (ModelState["Photo"] != null)
            {
                if (!brand.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Only image");
                    return View();
                }
                if (brand.Photo.ImageSize(800))
                {
                    ModelState.AddModelError("Photo", "Olcu oversize");
                    return View();
                }


                string path = Path.Combine(_env.WebRootPath, @"assets\images\brand", dbBrand.ImageUrl);
                Helpers.Helpers.DeleteImage(path);
                dbBrand.ImageUrl = "images/brand/" + brand.Photo.SaveImage(_env, @"assets\images\brand");
            }   
            var brandName = _context.Brands.FirstOrDefault(x=>x.Name.ToLower()==brand.Name.ToLower());

			if (brandName != null)
			{
				if (dbBrand.Name.ToLower() != brandName.Name.ToLower())
				{
                    ModelState.AddModelError("Name", "Model Name is exsist");
                    return View("Update");
                }
			}
            dbBrand.Name = brand.Name;
            dbBrand.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return  RedirectToAction("show");
		}

        public async Task<IActionResult> Remove(int? id)
        {
            if (id == null) return NotFound();

            Brand dbBrand =await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);
            if (dbBrand == null) return NotFound();

            //string path = Path.Combine(_env.WebRootPath, @"assets\images\brand", dbBrand.ImageUrl);
            //Helpers.Helpers.DeleteImage(path);

            dbBrand.IsDeleted = true;

            _context.Brands.Remove(dbBrand);

            await _context.SaveChangesAsync();


            return RedirectToAction("show");
        }
    }
}
