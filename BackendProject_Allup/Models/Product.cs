namespace BackendProject_Allup.Models
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsFeatured { get; set; }
        public bool BestSeller { get; set; }
        public bool NewArrival { get; set; }
        public bool InStock { get; set; }
        public double Price { get; set; }
        public double DiscountPrice { get; set; }
        public double TaxPercent { get; set; }
        public int StockCount { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }

        public List<ProductImage> ProductImages { get; set; }
        public List<TagProduct>? TagProducts { get; set; }  
        public List<OrderItem>? OrderItems { get; set; }
        public List<BasketItem>? BasketItems { get; set; }


    }
}
