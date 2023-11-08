namespace VendingMachine.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public double Deposit { get; set; }
        public string Role { get; set; }
    }
}
