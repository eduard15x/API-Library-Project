using AutoMapper;
using ITPLibrary.Api.Core.Dtos.ShoppingCart;
using ITPLibrary.Api.Data.Shared.Entities;
using ITPLibrary.Api.Data.Shared.IRepository;

namespace ITPLibrary.Api.Core.Services.ShoppingCart
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IMapper _mapper;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartService(IMapper mapper, IShoppingCartRepository shoppingCartRepository)
        {
            _mapper = mapper;
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<List<GetShoppingCartDto>> AddToShoppingCart(AddShoppingCartDto newItem)
        {
            var item = _mapper.Map<ShoppingCartModel>(newItem);
            await _shoppingCartRepository.AddToShoppingCart(item);

            var shoppingCartList = await _shoppingCartRepository.GetShoppingCart();
            return _mapper.Map<List<GetShoppingCartDto>>(shoppingCartList);
        }

        public async Task<List<GetShoppingCartDto>> GetShoppingCart()
        {
            var shoppingCartList = await _shoppingCartRepository.GetShoppingCart();
            return _mapper.Map<List<GetShoppingCartDto>>(shoppingCartList);
        }
    }
}
