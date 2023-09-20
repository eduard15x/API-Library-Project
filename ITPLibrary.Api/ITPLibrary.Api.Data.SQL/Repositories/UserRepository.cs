using Dapper;
using ITPLibrary.Api.Data.Shared.Entities;
using ITPLibrary.Api.Data.Shared.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace ITPLibrary.Api.Data.SQL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connectionString;
        public UserRepository(IDbConnection connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<User> RegisterUser(User newUser)
        { 
            // Hash and salt the password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            newUser.Password = hashedPassword;

            var insertQuery = @"
                INSERT INTO Users (Email, Password)
                VALUES (@Email, @Password);
                SELECT CAST(SCOPE_IDENTITY() AS int)";

            var userId = await _connectionString.QueryFirstOrDefaultAsync<int>(insertQuery, newUser);

            // Fetch the newly created user by their ID
            var selectQuery = "SELECT * FROM Users WHERE Id = @UserId";

            var createdUser = await _connectionString.QueryFirstOrDefaultAsync<User>(selectQuery, new { UserId = userId});

            return createdUser;
        }

        public async Task<User> LoginUser(string email)
        {
            var query = "SELECT * FROM Users WHERE Email = @Email";

            return await _connectionString.QueryFirstOrDefaultAsync<User>(query, new { Email = email });
        }

        public async Task UpdatePassword(string email, string newPassword)
        {
            // Hash the new password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

            string updateSql = "UPDATE Users SET Password = @Password WHERE Email = @Email";

            await _connectionString.ExecuteAsync(updateSql, new { Password = hashedPassword, Email = email });
        }

        public async Task<bool> CheckIfMailExistsInDatabase(string email)
        {
            string sql = "SELECT COUNT(*) FROM Users WHERE Email = @Email";

            int count = await _connectionString.ExecuteScalarAsync<int>(sql, new { Email = email });

            return count > 0;
        }
    }
}
