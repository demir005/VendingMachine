namespace VendingMachine.Extensions
{
    public class ValidationHelper
    {
        public static bool IsValidCoinValue(int coinValue)
        {
            // Define the valid coin values (5, 10, 20, 50, 100 cents)
            int[] validCoinValues = { 5, 10, 20, 50, 100 };

            // Check if the provided coin value is in the valid list
            return Array.IndexOf(validCoinValues, coinValue) >= 0;
        }
    }
}
