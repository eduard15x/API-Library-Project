using ITPLibrary.Api.Core.Dtos.FavoriteProduct;

namespace ITPLibrary.Api.Core.Services.FavoriteProductService
{
    public interface IFavoriteProductService
    {
        Task<List<GetFavoriteProductDto>> GetFavoriteProducts(int userId);
        Task<string> AddProductToFavorite(int productId, int userId);
        Task<string> DeleteProductFromFavorites(int productId, int userId);
    }
}
