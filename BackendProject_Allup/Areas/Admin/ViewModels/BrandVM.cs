using BackendProject_Allup.Models;

namespace BackendProject_Allup.Areas.Admin.ViewModels
{
    public class BrandVM
    {

		public int Id { get; set; }
		public string Name { get; set; }
        public string ImageUrl { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public IFormFile Photo { get; set; }
    }
}
