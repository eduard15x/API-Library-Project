using System.ComponentModel.DataAnnotations;

namespace ITPLibrary.Api.Core.Dtos.User
{
    public class UserTokenDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
