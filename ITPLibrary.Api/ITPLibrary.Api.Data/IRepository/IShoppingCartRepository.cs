using ITPLibrary.Api.Data.Shared.Entities;

namespace ITPLibrary.Api.Data.Shared.IRepository
{
    public interface IShoppingCartRepository
    {
        Task<List<ShoppingCartModel>> GetShoppingCart();
        Task<List<ShoppingCartModel>> AddToShoppingCart(ShoppingCartModel newItem);
        Task<List<ShoppingCartModel>> RemoveItemFromShoppingCart(int id);
        Task<string> RemoveAllItemsFromCart(int userId);
    }
}
