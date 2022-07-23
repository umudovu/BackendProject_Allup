namespace BackendProject_Allup.Models
{
    public class Category:BaseEntity
    {
        public string? Name { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string? ImageUrl { get; set; }
        public Category? Parent { get; set; }
        public List<Category>? Children { get; set; }

        public List<Product>? Products { get; set; }
    }
}
