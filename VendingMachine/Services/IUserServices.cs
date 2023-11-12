using VendingMachine.Models;
using VendingMachine.Models.Dto;

namespace VendingMachine.Services
{
    public interface IUserServices
    {
        Task<(int, string)> Registeration(RegistrationModel model);
        Task<(int, string)> Login(LoginModel model);
        Task<bool> LogoutAllSessions(int userId);
        Task<ApplicationUser> GetUserById(string userId);
        Task<bool> UpdateUser(int userId, UpdateUserModel model);
        Task<bool> DeleteUser(int userId);
        Task<IEnumerable<ApplicationUser>> GetAllUsers(); // doesnt work 

        
    }
}
