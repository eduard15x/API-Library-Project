using ITPLibrary.Api.Data.Shared.Entities;

namespace ITPLibrary.Api.Data.Shared.IRepository
{
    public interface IFavoriteProductRepository
    {
        Task<List<FavoriteProduct>> GetFavoriteProducts(int userId);
        Task<string> AddProductToFavorite(int productId, int userId);
        Task<string> DeleteProductFromFavorites(int productId, int userId);
    }
}
