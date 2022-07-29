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
    public class CategoryController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(AppDbContext context,  IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Show(int page = 1, int pageSize = 10)
        {

            List<Category> categories = _context.Categories.Where(x=>x.IsDeleted==false).OrderBy(x=>x.Id).ToList();

            ViewBag.Parents = _context.Categories.Where(c => c.ParentId == null).ToList();

            var list = await PagedList<Category>.CreateAsync(categories, page, pageSize);

            return View(list);
        }

        public IActionResult Create()
        {
            var dbParent = _context.Categories.Where(x=>x.ParentId==null).ToList();
            ViewBag.Categories = new SelectList(dbParent, "Id", "Name");

            CategoryVM category = new CategoryVM();

            return View(category);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CategoryVM category)
		{
			if (ModelState["Photo"] == null)
			{
				ModelState.AddModelError("Photo", "Photo is null");
				return View();
			}
			if (!category.Photo.IsImage())
			{
				ModelState.AddModelError("Photo", "Only image");
				return View();
			}
			if (category.Photo.ImageSize(800))
			{
				ModelState.AddModelError("Photo", "Olcu oversize");
				return View();
			}


			if (_context.Categories.Any(c => c.Name.ToLower() == category.Name.ToLower()))
			{
				ModelState.AddModelError("Name", "Category name is exist!");
				return View();
			}

			Category newCategory = new Category()
			{
				Name = category.Name,
				ParentId = category.ParentId,
				ImageUrl = "images/" + category.Photo.SaveImage(_env, @"assets\images"),
				CreatedAt = DateTime.Now

			};


			await _context.Categories.AddAsync(newCategory);
			await _context.SaveChangesAsync();

			return RedirectToAction("show");
		}

		public IActionResult Update(int? id)
		{
			if (id == null) return NotFound();

			Category dbCategory = _context.Categories.FirstOrDefault(b => b.Id == id);
			if (dbCategory == null) return NotFound();

			CategoryVM category = new CategoryVM()
			{
				Id = dbCategory.Id,
				Name = dbCategory.Name,
			};

			var dbParent = _context.Categories.Where(x => x.ParentId == null).ToList();
			ViewBag.Categories = new SelectList(dbParent, "Id", "Name");

			return View(category);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(CategoryVM category,string ReturnUrl)
		{
			Category dbCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
			if (dbCategory == null) return NotFound();

			if (category.Photo != null)
			{
				if (!category.Photo.IsImage())
				{
					ModelState.AddModelError("Photo", "Only image");
					return View();
				}
				if (category.Photo.ImageSize(800))
				{
					ModelState.AddModelError("Photo", "Olcu oversize");
					return View();
				}


				string path = Path.Combine(_env.WebRootPath, @"assets\images", dbCategory.ImageUrl);
				Helpers.Helpers.DeleteImage(path);
				dbCategory.ImageUrl = "images/brand/" + category.Photo.SaveImage(_env, @"assets\images\brand");
			}

			var categoryName = _context.Categories.FirstOrDefault(x => x.Name.ToLower() == category.Name.ToLower());

			if (categoryName != null)
			{
				if (dbCategory.Name.ToLower() != categoryName.Name.ToLower())
				{
					ModelState.AddModelError("Name", "Model Name is exsist");
					return View("Update");
				}
			}
			dbCategory.Name = category.Name;
			dbCategory.ParentId = category.ParentId;
			dbCategory.UpdatedAt = DateTime.Now;

			await _context.SaveChangesAsync();
			return Redirect(ReturnUrl);
		}


		public async Task<IActionResult> Remove(int? id,string ReturnUrl)
		{
			if (id == null) return NotFound();

			Category dbCategory = _context.Categories.FirstOrDefault(c => c.Id == id);
			if (dbCategory == null) return NotFound();

			//string path = Path.Combine(_env.WebRootPath, @"assets\images\brand", dbBrand.ImageUrl);
			//Helpers.Helpers.DeleteImage(path);

			dbCategory.IsDeleted = true;

			_context.Categories.Remove(dbCategory);

			await _context.SaveChangesAsync();


			return Redirect(ReturnUrl);
		}
	}
}
