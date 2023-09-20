using ITPLibrary.Api.Core.Dtos.User;

namespace ITPLibrary.Api.Core.Services.UserService
{
    public interface IUserService
    {
        Task<UserDto> RegisterUser (UserDto user);
        Task<UserTokenDto> LoginUser (UserLoginDto loginDto);
        Task UpdatePasswordInDatabase(string email);
    }
}
