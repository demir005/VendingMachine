using Microsoft.AspNetCore.Identity;

namespace VendingMachine.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Deposit { get; set; }
        public string SessionToken { get; set; }
    }
}
