using ITPLibrary.Api.Core.Dtos.ShoppingCart;

namespace ITPLibrary.Api.Core.Services.ShoppingCart
{
    public interface IShoppingCartService
    {
        Task<List<GetShoppingCartDto>> GetShoppingCart();
        Task<List<GetShoppingCartDto>> AddToShoppingCart(AddShoppingCartDto newItem);
    }
}
