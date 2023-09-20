using ITPLibrary.Api.Data.Shared.Entities;

namespace ITPLibrary.Api.Data.Shared.IRepository
{
    public interface IOrderRepository
    {
        Task<OrderDetails> GetUserOrder(int orderDetailsId);
        Task<List<Order>> GetUserOrderProducts(int orderDetailsId);
        Task<OrderDetails> CreateNewOrder(OrderDetails newOrderDetails);
    }
}
