using ITPLibrary.Api.Data.EF.Data;
using ITPLibrary.Api.Data.Shared.Entities;
using ITPLibrary.Api.Data.Shared.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ITPLibrary.Api.Data.EF.Repositories
{
    public class FavoriteProductRepository : IFavoriteProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FavoriteProductRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        public async Task<List<FavoriteProduct>> GetFavoriteProducts(int userId)
        {
            var currentUser = GetUserId();

            if (userId != currentUser)
            {
                throw new Exception("Not authorized.");
            }

            var favoriteProductsList = await _context.FavoriteProducts
                .Where(item => item.UserId == currentUser)
                .ToListAsync();

            if (favoriteProductsList.Count <= 0)
            {
                return null!;
            }
            else
            {
                return favoriteProductsList;
            }
        }

        public async Task<string> AddProductToFavorite(int productId, int userId)
        {
            var currentUser = GetUserId();

            if (userId != currentUser)
            {
                throw new Exception("Not authorized.");
            }

            var productExistsInFavoriteList = await _context.FavoriteProducts
                .FirstOrDefaultAsync(item => item.BookId == productId);

            if (productExistsInFavoriteList != null)
            {
                throw new Exception("Product already exists.");
            }

            var newFavoriteProduct = new FavoriteProduct()
            {
                UserId = userId,
                BookId = productId
            };

            _context.FavoriteProducts.Add(newFavoriteProduct);
            await _context.SaveChangesAsync();

            return "Product added to favorite.";
        }

        public async Task<string> DeleteProductFromFavorites(int productId, int userId)
        {
            var currentUser = GetUserId();

            if (userId != currentUser)
            {
                throw new Exception("Not authorized.");
            }

            var productExistsInFavoriteList = await _context.FavoriteProducts
                .FirstOrDefaultAsync(item => item.BookId == productId && item.UserId == userId);

            if (productExistsInFavoriteList is null)
            {
                throw new Exception("Product is not in the favorite list.");
            }


            _context.FavoriteProducts.Remove(productExistsInFavoriteList);
            await _context.SaveChangesAsync();

            return "Product deleted from favorites.";
        }
    }
}
