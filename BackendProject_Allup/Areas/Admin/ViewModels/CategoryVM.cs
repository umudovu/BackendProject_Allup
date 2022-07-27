namespace BackendProject_Allup.Areas.Admin.ViewModels
{
	public class CategoryVM
	{
		public int Id { get; set; }
		public string? Name { get; set; }
        public Nullable<int> ParentId { get; set; }
		public IFormFile Photo { get; set; }

	}
}
