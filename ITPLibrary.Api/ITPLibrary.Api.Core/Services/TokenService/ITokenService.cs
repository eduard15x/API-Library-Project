using ITPLibrary.Api.Data.Shared.Entities;

namespace ITPLibrary.Api.Core.Services.TokenService
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
