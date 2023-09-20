using System.ComponentModel.DataAnnotations;

namespace ITPLibrary.Api.Core.Dtos.User
{
    public class UserLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
