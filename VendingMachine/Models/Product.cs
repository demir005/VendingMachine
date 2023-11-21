using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VendingMachine.Extensions;

namespace VendingMachine.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string SellerId { get; set; }
        [JsonIgnore]
        public ApplicationUser Seller { get; set; }
        public int AmountAvailable { get; set; }

        [CostValidation]
        [Range(0, int.MaxValue)]
        public int Cost { get; set; }


    }
}
