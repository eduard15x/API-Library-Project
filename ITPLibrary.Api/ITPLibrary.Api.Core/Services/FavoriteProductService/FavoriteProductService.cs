using AutoMapper;
using ITPLibrary.Api.Core.Dtos.FavoriteProduct;
using ITPLibrary.Api.Data.Shared.IRepository;

namespace ITPLibrary.Api.Core.Services.FavoriteProductService
{
    public class FavoriteProductService : IFavoriteProductService
    {
        private readonly IMapper _mapper;
        private readonly IFavoriteProductRepository _favoriteProductRepository;

        public FavoriteProductService(IMapper mapper, IFavoriteProductRepository favoriteProductRepository)
        {
            _mapper = mapper;
            _favoriteProductRepository = favoriteProductRepository;
        }

        public async Task<List<GetFavoriteProductDto>> GetFavoriteProducts(int userId)
        {
            if (userId <= 0)
            {
                throw new Exception("User doesn't exist.");
            }

            var favoriteProductList =  await _favoriteProductRepository.GetFavoriteProducts(userId);
            
            return _mapper.Map<List<GetFavoriteProductDto>>(favoriteProductList);
        }

        public async Task<string> AddProductToFavorite(int productId, int userId)
        {
            if (userId <= 0)
            {
                throw new Exception("User doesn't exist.");
            }

            return await _favoriteProductRepository.AddProductToFavorite(productId, userId);
        }

        public async Task<string> DeleteProductFromFavorites(int productId, int userId)
        {
            if (userId <= 0)
            {
                throw new Exception("User doesn't exist.");
            }

            return await _favoriteProductRepository.DeleteProductFromFavorites(productId, userId);
        }
    }
}
