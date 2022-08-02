using BackendProject_Allup.DAL;
using BackendProject_Allup.Helpers;
using BackendProject_Allup.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BackendProject_Allup.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public UserController(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager,
            AppDbContext context, IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            _config = config;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users
                    .Include(x=>x.Orders)
                    .ToListAsync();

            return View(users);
        }
        public async Task<IActionResult> Detail(string id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users
                .Include(x=>x.Orders)
                .ThenInclude(x=>x.OrderItems)
                .FirstOrDefaultAsync(x => x.Id == id);

            ViewBag.Role = await _userManager.GetRolesAsync(user);

            ViewBag.Roles =_roleManager.Roles.ToList();
            if (user == null) return NotFound();
            

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(string roleid, string userid)
        {
            if (roleid == null) return NoContent();

            var user = _userManager.Users.FirstOrDefault(x => x.Id == userid);

            var role =await _roleManager.Roles.FirstOrDefaultAsync(x=>x.Id== roleid);

            var userrole = await _userManager.AddToRoleAsync(user,role.Name);

            return RedirectToAction("index");
        }
    }
}
