using BackendProject_Allup.Models;

namespace BackendProject_Allup.Areas.Admin.ViewModels
{
    public class DashboardVM
    {
        public List<AppUser> Users { get; set; }
        public List<Product> Products { get; set; }
        public double OrderTotalprice { get; set; }
    }
}
