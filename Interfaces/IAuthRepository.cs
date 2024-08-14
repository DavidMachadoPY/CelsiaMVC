using Celsia.Models;
using Celsia.ViewModels;

namespace Celsia.Interfaces
{
    public interface IAuthRepository
    {
            Task<User> Register(UserCreateViewModel user);
            Task<User> Login(UserLoginViewModel user);
            Task<User> UpdateUser(UserUpdateViewModel userUpdate, string userId);
            // Task<User> GetUserById(string userId);
            // Task<bool> DeleteUser(string userId);
    }
}