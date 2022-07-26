using BackendProject_Allup.DAL;
using BackendProject_Allup.Models;
using BackendProject_Allup.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static BackendProject_Allup.Helpers.Helpers;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace BackendProject_Allup.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, AppDbContext context = null)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser appUser = new AppUser
            {
                FullName = registerVM.FullName,
                Email = registerVM.Email,
                UserName = registerVM.UserName,

            };

            IdentityResult result = await _userManager.CreateAsync(appUser, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(registerVM);
            }

            

            await _userManager.AddToRoleAsync(appUser, UserRoles.Member.ToString());
            return RedirectToAction("login");
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM, string ReturnUrl)
        {
            //if (!ModelState.IsValid) return View();

            var user = User.FindFirst(ClaimTypes.Name)?.Value;

            AppUser appUser = await _userManager.FindByEmailAsync(loginVM.Email);
            if (appUser == null)
            {
                ModelState.AddModelError("", "Email veya password invalid");
                return View(loginVM);
            }



            SignInResult result = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, true, true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Email veya password invalid");
                return View(loginVM);
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email veya password invalid");
                return View(loginVM);
            }

            var roles = await _userManager.GetRolesAsync(appUser);

            if (ReturnUrl != null)
            {
                foreach (var item in roles)
                {
                    if (item == "Admin")
                    {
                        return RedirectToAction("Index", "dashboard", new { area = "Admin" });
                    }
                    else
                    {
                        if (ReturnUrl == "register")
                        {
                            return RedirectToAction("index", "home");
                        }
                        return Redirect(ReturnUrl); ;
                    }
                }
            }

            Basket basket = _context.Baskets
                   .Include(b => b.BasketItems)
                   .FirstOrDefault(b => b.UserId == appUser.Id);


            if (basket == null)
            {
                Basket b = new Basket() { UserId = appUser.Id };
                _context.Add(b);
                _context.SaveChanges();
            }

            return RedirectToAction("index","home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }



        public async Task CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(UserRoles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
                }
            };
        }


        //public async Task<ActionResult> CreateUserFake()
        //{

        //    List<AppUser> _users = new Faker<AppUser>()
        //                .RuleFor(x => x.UserName, f => f.Name.FirstName())
        //                .RuleFor(x => x.Fullname, f => f.Name.FullName())
        //                .RuleFor(x => x.Email, f => f.Person.Email)
        //                .Generate(100);


        //    foreach (var item in _users)
        //    {
        //        await _userManager.CreateAsync(item, "Pa$$word123");
        //        await _userManager.AddToRoleAsync(item, UserRoles.Member.ToString());
        //    }



        //    return RedirectToAction("Index", "Home");
        //}
    }
}
