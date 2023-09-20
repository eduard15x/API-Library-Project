using ITPLibrary.Api.Core.Dtos.Order;

namespace ITPLibrary.Api.Core.Services.OrderService
{
    public interface IOrderService
    {
        Task<UserSpecificOrderDto> GetUserOrder(int orderDetailsId);
        Task<UserSpecificOrderDto> CreateNewOrder(AddNewOrderDto newOrderDetails);
    }
}
