namespace VendingMachine.Models.Dto
{
    public class ReceiptDTO
    {
        public int TotalSpent { get; set; }
        public string ProductPurchased { get; set; }
        public Dictionary<int, int> Change { get; set; }
    }
}
