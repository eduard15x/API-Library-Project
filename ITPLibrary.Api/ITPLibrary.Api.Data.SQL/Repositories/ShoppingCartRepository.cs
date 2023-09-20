using Dapper;
using ITPLibrary.Api.Data.Shared.Entities;
using ITPLibrary.Api.Data.Shared.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace ITPLibrary.Api.Data.SQL.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly IDbConnection _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCartRepository(IDbConnection connectionString, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = connectionString;
            _httpContextAccessor = httpContextAccessor;
        }
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<List<ShoppingCartModel>> AddToShoppingCart(ShoppingCartModel newItem)
        {
            var currentUserId = GetUserId();

            var productExists = await _connectionString.QueryFirstOrDefaultAsync<Book>(
                "SELECT * From Books Where Id = @Id",
                new { Id = newItem.BookId }
            );

            var currentShoppingCart = await _connectionString.QueryFirstOrDefaultAsync<ShoppingCartModel>(
                "SELECT * From ShoppingCarts Where BookId = @BookId AND UserId = @UserId",
                new { BookId = newItem.BookId, UserId = currentUserId }
            );

            if (productExists is null)
            {
                throw new Exception($"Product with id {newItem.Id} doesn't exist.");
            }

            if (currentShoppingCart is null)
            {
                newItem.UserId = currentUserId;
                newItem.CartPrice = productExists.Price;

                await _connectionString.ExecuteAsync(
                    "INSERT INTO ShoppingCarts (UserId, BookId, Count, CartPrice) Values (@UserId, @BookId, @Count, @CartPrice)",
                    new { UserId = newItem.UserId, BookId = newItem.BookId, Count = newItem.Count, CartPrice = newItem.CartPrice }
                );
            } 
            else // update current count of products
            {
                await _connectionString.ExecuteAsync(
                    "UPDATE ShoppingCarts SET Count = @Count WHERE Id = @Id",
                    new { Count = currentShoppingCart.Count + newItem.Count, Id = currentShoppingCart.Id }
                );
            }

            var newShoppingCart = await _connectionString.QueryAsync<ShoppingCartModel>(
                "SELECT * From ShoppingCarts WHERE UserId = @UserId",
                new { UserId = currentUserId }
            );

            return newShoppingCart.ToList();
        }

        public async Task<List<ShoppingCartModel>> GetShoppingCart()
        {
            var currentUserId = GetUserId();
            var query = "SELECT * From ShoppingCarts Where UserId = @UserId";

            return (await _connectionString.QueryAsync<ShoppingCartModel>(query, new { UserId = currentUserId })).ToList();
        }

        public async Task<List<ShoppingCartModel>> RemoveItemFromShoppingCart(int productId)
        {
            var currentUserId = GetUserId();
            var productExistsInShoppingCart = await _connectionString.QueryFirstOrDefaultAsync<ShoppingCartModel>(
                "SELECT * FROM ShoppingCarts WHERE BookId = @BookId AND UserId = @UserId",
                new { BookId = productId, UserId = currentUserId }
            );

            if (productExistsInShoppingCart is null)
            {
                throw new Exception($"Product with id '{productId}' not existing in shopping cart.");
            }

            await _connectionString.ExecuteAsync(
                "DELETE FROM ShoppingCarts Where BookId = @BookId AND UserId = @UserID",
                new { BookId = productId, UserId = currentUserId }
            );

            var newShoppingCart = await _connectionString.QueryAsync<ShoppingCartModel>(
                "SELECT * FROM ShoppingCarts WHERE UserId = @UserId",
                new { UserId = currentUserId }
            );

            return newShoppingCart.ToList();
        }

        public async Task<string> RemoveAllItemsFromCart(int userId)
        {
            var currentUserId = GetUserId();

            if (userId <= 0)
            {
                throw new Exception("User doesn't exist.");
            }
            else if (userId != currentUserId)
            {
                throw new Exception("User not authorized.");
            }

            var shoppingCartList = await _connectionString.QueryAsync<ShoppingCartModel>(
                "SELECT * FROM ShoppingCarts Where UserId = @UserId",
                new { UserId = userId }
            );

            if (shoppingCartList != null && shoppingCartList.Any())
            {
                await _connectionString.ExecuteAsync(
                    "DELETE From ShoppingCarts Where UserId = @UserId",
                    new { UserId = userId }
                );

                return "Items from cart were deleted.";
            }
            else
            {
                return "Cart is empty.";
            }
        }
    }
}
