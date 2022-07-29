using BackendProject_Allup.Models;

namespace BackendProject_Allup.ViewModels
{
    public class MyAccountVM
    {
        public List<Order> Orders { get; set; }
        public AppUser User { get; set; }
    }
}
