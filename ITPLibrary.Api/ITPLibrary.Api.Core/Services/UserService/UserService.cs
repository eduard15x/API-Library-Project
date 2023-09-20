using AutoMapper;
using ITPLibrary.Api.Core.Dtos.Email;
using ITPLibrary.Api.Core.Dtos.User;
using ITPLibrary.Api.Core.Services.EmailService;
using ITPLibrary.Api.Core.Services.TokenService;
using ITPLibrary.Api.Data.Shared.Entities;
using ITPLibrary.Api.Data.Shared.IRepository;
using System.Diagnostics;
using System.Security.Cryptography;

namespace ITPLibrary.Api.Core.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public UserService(IMapper mapper, IUserRepository userRepository, ITokenService tokenService, IEmailService emailService)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        public async Task<UserDto> RegisterUser(UserDto newUser)
        {
            if (newUser.Password != newUser.ConfirmPassword)
            {
                throw new InvalidOperationException("Passwords don't match.");
            }

            var user = _mapper.Map<User>(newUser);

            var registeredUser = await _userRepository.RegisterUser(user);

            return _mapper.Map<UserDto>(registeredUser);
        }

        public async Task<UserTokenDto> LoginUser(UserLoginDto loginDto)
        {
            var user = await _userRepository.LoginUser(loginDto.Email);

            if (user == null)
            {
                return null!;
            }

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);
            if (!isPasswordValid)
            {
                return null!;
            }

            return new UserTokenDto
            {
                Email = user.Email,
                Token = _tokenService.GenerateToken(user)
            };
        }

        public async Task UpdatePasswordInDatabase(string email)
        {
            var isUserExist = await _userRepository.CheckIfMailExistsInDatabase(email);

            if (!isUserExist)
            {
                throw new Exception($"User with email '{email}' was not found.");
            }
            var randomPassword = GenerateRandomPassword();

            await _userRepository.UpdatePassword(email, randomPassword);
            SendPasswordResetEmail(email, randomPassword);
        }

        private static string GenerateRandomPassword(int length = 12)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-_=+[]{}|;:,.<>?";
            char[] chars = new char[length];

            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                for (int i = 0; i < length; i++)
                {
                    int index = randomBytes[i] % validChars.Length;
                    chars[i] = validChars[index];
                }
            }

            return new string(chars);
        }

        private void SendPasswordResetEmail(string emailAddress, string newPassword)
        {
            var emailDto = new EmailDto
            {
                ToEmail = emailAddress,
                Subject = "Password Reset Web Core API",
                Body = $"Your new password is: <b>{newPassword}</b>" // Customize the email body as needed
            };

            // Use your EmailService to send the email
            _emailService.SendEmail(emailDto);
        }
    }
}
