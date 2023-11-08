using System.ComponentModel.DataAnnotations;
using VendingMachine.Extensions;

namespace VendingMachine.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int SellerId { get; set; }
        public int AmountAvailable { get; set; }

        [CostValidation]
        [Range(0, int.MaxValue)]
        public int Cost { get; set; }


    }
}
