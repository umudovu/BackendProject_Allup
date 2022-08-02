using BackendProject_Allup.Models;

namespace BackendProject_Allup.ViewModels
{
    public class ShopVM
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public Product Product { get; set; }
        public List<Banner> Banners { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Comment> Comments { get; set; }
        public string Username { get; set; }
        public Basket UserBakset { get; set; }
        public int UserBasketProductCount { get; set; }

    }
}
