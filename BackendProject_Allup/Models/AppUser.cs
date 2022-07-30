using Microsoft.AspNetCore.Identity;

namespace BackendProject_Allup.Models
{
    public class AppUser:IdentityUser
    {
        public string? FullName { get; set; }

        public double Balance { get; set; }
        public DateTime Craeted { get; set; }
        public List<Order>? Orders { get; set; }
        public Basket? Basket { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
