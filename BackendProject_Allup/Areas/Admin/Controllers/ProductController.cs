using BackendProject_Allup.DAL;
using BackendProject_Allup.Helpers;
using BackendProject_Allup.Models;
using Microsoft.AspNetCore.Mvc;
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
			List<Product> products = await _context.Products.Include(x => x.ProductImages)
				.Include(x => x.Brand)
				.Include(x => x.TagProducts)
				.Include(x => x.Category)
				.ToListAsync();


			var list = await PagedList<Product>.CreateAsync(products, page, pageSize);

			return View(list);
		}
	}
}
