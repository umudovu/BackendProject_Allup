using BackendProject_Allup.Models;

namespace BackendProject_Allup.Areas.Admin.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsFeatured { get; set; }
        public bool BestSeller { get; set; }
        public bool NewArrival { get; set; }
        public double Price { get; set; }
        public double DiscountPrice { get; set; }
        public double TaxPercent { get; set; }
        public int StockCount { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public List<ProductImage> Images { get; set; }
        public List<IFormFile> Photos { get; set; }
    }
}
