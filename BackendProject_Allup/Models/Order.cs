namespace BackendProject_Allup.Models
{
    public class Order:BaseEntity
    {
        public string? Address { get; set; }
        public string? Email { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public string? UserId { get; set; }
        public AppUser User { get; set; }
    }


    public enum OrderStatus
    {
        Pendibg,
        Shipped
    }
}
