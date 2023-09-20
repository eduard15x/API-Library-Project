using ITPLibrary.Api.Data.Shared.Entities;

namespace ITPLibrary.Api.Data.Shared.IRepository
{
    public interface IUserRepository
    {
        Task<User> RegisterUser(User newUser);
        Task<User> LoginUser(string email);
        Task UpdatePassword(string email, string newPassword);
        Task<bool> CheckIfMailExistsInDatabase(string email);
    }
}
