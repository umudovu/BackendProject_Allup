namespace BackendProject_Allup.Models
{
    public class Brand:BaseEntity
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public List<Product> Products { get; set; }

    }
}
