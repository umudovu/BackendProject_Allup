namespace BackendProject_Allup.Models
{
    public class Comment:BaseEntity
    {
        public string Content { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
