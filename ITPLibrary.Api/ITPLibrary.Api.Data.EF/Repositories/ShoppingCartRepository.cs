using ITPLibrary.Api.Data.EF.Data;
using ITPLibrary.Api.Data.Shared.Entities;
using ITPLibrary.Api.Data.Shared.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ITPLibrary.Api.Data.EF.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCartRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<List<ShoppingCartModel>> GetShoppingCart()
        {
            return await _context.ShoppingCarts
                .Where(c => c.UserId == GetUserId())
                .ToListAsync();
        }

        public async Task<List<ShoppingCartModel>> AddToShoppingCart(ShoppingCartModel newItem)
        {
            var currentUserId = GetUserId();
            var productExists = await _context.Books.FirstOrDefaultAsync(p => p.Id == newItem.BookId);
            var currentShoppingCart = await _context.ShoppingCarts.FirstOrDefaultAsync(c => c.BookId == newItem.BookId && c.UserId == currentUserId);

            if (productExists is null)
            {
                throw new Exception($"Product with id {newItem.Id} doesn't exist.");
            }

            if (currentShoppingCart is null)
            {
                newItem.UserId = currentUserId;
                newItem.CartPrice = productExists.Price;
                _context.ShoppingCarts.Add(newItem);
            } 
            else
            {
                currentShoppingCart.Count = currentShoppingCart.Count + newItem.Count;
            }

            await _context.SaveChangesAsync();
            return await _context.ShoppingCarts.ToListAsync();
        }

        public async Task<List<ShoppingCartModel>> RemoveItemFromShoppingCart(int productId)
        {
            var currentUserId = GetUserId();
            var productExistsInShoppingCart = await _context.ShoppingCarts.FirstOrDefaultAsync(p => p.BookId == productId && p.UserId == currentUserId);

            if ( productExistsInShoppingCart is null)
            {
                throw new Exception($"Product with id '{productId}' not existing in shopping cart.");
            }

            _context.ShoppingCarts.Remove(productExistsInShoppingCart);
            await _context.SaveChangesAsync();

            return await _context.ShoppingCarts
                .Where(c => c.UserId == GetUserId())
                .ToListAsync();
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

            var shoppingCartList = await _context.ShoppingCarts
                .Where(item => item.UserId == userId)
                .ToListAsync();

            if (shoppingCartList.Any())
            {
                _context.ShoppingCarts.RemoveRange(shoppingCartList);
                await _context.SaveChangesAsync();
                return "Items from cart were deleted.";
            }
            else
            {
                return "Cart is empty.";
            }
        }
    }
}
