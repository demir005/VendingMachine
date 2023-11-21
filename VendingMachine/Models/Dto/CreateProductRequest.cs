using System.ComponentModel.DataAnnotations;

namespace VendingMachine.Models.Dto
{
    public class CreateProductRequest
    {
        [Required]
        public string ProductName { get; set; }

        [Required]
        public string SellerId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Amount Available must be greater than 0.")]
        public int AmountAvailable { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Cost must be greater than 0 and in multiples of 5.")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Cost must be in multiples of 5.")]
        public int Cost { get; set; }
    }
}
