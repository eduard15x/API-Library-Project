using ITPLibrary.Api.Data.EF.Data;
using ITPLibrary.Api.Data.Shared.Entities;
using ITPLibrary.Api.Data.Shared.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ITPLibrary.Api.Data.EF.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext _context)
        {
            this._context = _context;
        }
        public async Task<User> RegisterUser(User newUser)
        {
            // Check if the user already exists with the same email
            if (_context.Users.Any(u => u.Email == newUser.Email))
            {
                throw new InvalidOperationException("User with this email already exists.");
            }

            // Hash and salt the password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            newUser.Password = hashedPassword;

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }
        public async Task<User> LoginUser(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> CheckIfMailExistsInDatabase(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task UpdatePassword(string email, string newPassword)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (existingUser == null)
            {
                throw new InvalidOperationException($"User with email '{email}' was not found.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
            existingUser.Password = hashedPassword;

            await _context.SaveChangesAsync();
        }
    }
}
