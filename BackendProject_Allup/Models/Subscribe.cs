using System.ComponentModel.DataAnnotations;

namespace BackendProject_Allup.Models
{
    public class Subscribe
    {
        public int Id { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
