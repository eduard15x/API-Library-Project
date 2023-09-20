using ITPLibrary.Api.Core.Dtos.Email;

namespace ITPLibrary.Api.Core.Services.EmailService
{
    public interface IEmailService
    {
        void SendEmail(EmailDto emailDto);
    }
}
