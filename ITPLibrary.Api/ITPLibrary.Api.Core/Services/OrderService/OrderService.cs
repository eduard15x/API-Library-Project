using AutoMapper;
using ITPLibrary.Api.Core.Dtos.Order;
using ITPLibrary.Api.Data.Shared.Entities;
using ITPLibrary.Api.Data.Shared.IRepository;

namespace ITPLibrary.Api.Core.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<UserSpecificOrderDto> GetUserOrder(int orderDetailsId)
        {
            if (orderDetailsId <= 0)
            {
                throw new Exception("This order doesn't exist.");
            }

            var userOrderDetails = await _orderRepository.GetUserOrder(orderDetailsId); 
            var userOrderDetailsProducts = await _orderRepository.GetUserOrderProducts(orderDetailsId);

            var currentOrder = new UserSpecificOrderDto();
            currentOrder.OrderDetails = _mapper.Map<GetOrderDetailsDto>(userOrderDetails);
            currentOrder.Orders = _mapper.Map<List<OrderDto>>(userOrderDetailsProducts);

            return currentOrder;
        }

        public async Task<UserSpecificOrderDto> CreateNewOrder(AddNewOrderDto newOrderDetails)
        {
            var mappedNewOrderDetails = _mapper.Map<OrderDetails>(newOrderDetails);
            var userNewOrderDetails = await _orderRepository.CreateNewOrder(mappedNewOrderDetails);
            var userOrderDetailsProducts = await _orderRepository.GetUserOrderProducts(userNewOrderDetails.Id);

            var currentOrder = new UserSpecificOrderDto();
            currentOrder.OrderDetails = _mapper.Map<GetOrderDetailsDto>(userNewOrderDetails);
            currentOrder.Orders = _mapper.Map<List<OrderDto>>(userOrderDetailsProducts);

            return currentOrder;
        }
    }
}
