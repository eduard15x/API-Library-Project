using Dapper;
using ITPLibrary.Api.Data.Shared.Entities;
using ITPLibrary.Api.Data.Shared.IRepository;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Security.Claims;

namespace ITPLibrary.Api.Data.SQL.Repositories
{
    public class FavoriteProductRepository : IFavoriteProductRepository
    {
        private readonly IDbConnection _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FavoriteProductRepository(IDbConnection connectionString, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = connectionString;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<List<FavoriteProduct>> GetFavoriteProducts(int userId)
        {
            var currentUserId = GetUserId();

            if (userId <= 0 || currentUserId != userId)
            {
                throw new Exception("Not authorized.");
            }

            var favoriteProductsList = (await _connectionString.QueryAsync<FavoriteProduct>(
                @"SELECT * FROM FavoriteProducts
                WHERE UserId = @UserId",
                new { UserId = userId }
            )).ToList();

            if (favoriteProductsList.Count == 0)
            {
                throw new Exception("No favorite products in the list.");
            }

            return favoriteProductsList;
        }

        public async Task<string> AddProductToFavorite(int productId, int userId)
        {
            var currentUserId = GetUserId();

            if (userId <= 0 || currentUserId != userId)
            {
                throw new Exception("Not authorized.");
            }

            var productExistsInDb = await _connectionString.QueryFirstOrDefaultAsync<FavoriteProduct>(
                @"SELECT * FROM Books
                WHERE Id = @Id",
                new { Id = productId }
            );

            if (productExistsInDb == null)
            {
                return "Product doesn't exist.";
            }

            var productExistsInFavorite = await _connectionString.QueryFirstOrDefaultAsync<FavoriteProduct>(
                @"SELECT * FROM FavoriteProducts
                WHERE UserId = @UserId AND BookId = @BookId",
                new { UserId = userId, BookId = productId}
            );

            if (productExistsInFavorite != null)
            {
                return "Product already to favorite.";
            }

            await _connectionString.ExecuteAsync(
                @"INSERT INTO FavoriteProducts
                (UserId, BookId) VALUES (@UserId, @BookId)",
                new {UserId = userId, BookId = productId}
            );

            return "Product added to favorite";
        }

        public async Task<string> DeleteProductFromFavorites(int productId, int userId)
        {
            var currentUserId = GetUserId();

            if (userId <= 0 || currentUserId != userId)
            {
                throw new Exception("Not authorized.");
            }

            var productExistsInFavorite = await _connectionString.QueryFirstOrDefaultAsync<FavoriteProduct>(
                @"SELECT * FROM FavoriteProducts
                WHERE UserId = @UserId AND BookId = @BookId",
                new { UserId = userId, BookId = productId }
            );

            if (productExistsInFavorite == null)
            {
                return "Product is not in favorite list.";
            }

            await _connectionString.ExecuteAsync(
                @"DELETE FROM FavoriteProducts
                WHERE UserId = @UserId AND BookId = @BookId",
                new { UserId = userId, BookId = productId }
            );
            return "Product deleted from favorite";
        }
    }
}
