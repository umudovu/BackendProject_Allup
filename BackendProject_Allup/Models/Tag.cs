namespace BackendProject_Allup.Models
{
    public class Tag:BaseEntity
    {
        public string? Name { get; set; }

        public List<TagProduct>? TagProducts { get; set; }
    }
}
