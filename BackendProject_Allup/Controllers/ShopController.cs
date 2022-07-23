using Microsoft.AspNetCore.Mvc;

namespace BackendProject_Allup.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
