using Microsoft.AspNetCore.Identity;

namespace BackendProject_Allup.Models
{
    public class AppUser:IdentityUser
    {
        public string? FullName { get; set; }

        public DateTime Craeted { get; set; }
        public List<Order>? Orders { get; set; }
        public List<BasketItem>? BasketItems { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
