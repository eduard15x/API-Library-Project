using System.ComponentModel.DataAnnotations;

namespace ITPLibrary.Api.Core.Dtos.Email
{
    public class RecoverPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
