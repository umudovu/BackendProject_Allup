using BackendProject_Allup.Models;

namespace BackendProject_Allup.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }   
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public List<Brand> Brands { get; set; }

    }
}
