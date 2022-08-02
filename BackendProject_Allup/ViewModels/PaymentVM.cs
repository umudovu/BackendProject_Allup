using System.ComponentModel.DataAnnotations;

namespace BackendProject_Allup.ViewModels
{
    public class PaymentVM
    {
        [Required]
        public string Fullname { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public int CardNumber { get; set; }
        [Required]
        public int Month { get; set; }
        [Required]
        public int Year { get; set; }

        [Required]
        public int CVV { get; set; }
    }
}
