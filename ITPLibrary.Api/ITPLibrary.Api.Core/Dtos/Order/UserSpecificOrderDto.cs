namespace ITPLibrary.Api.Core.Dtos.Order
{
    public class UserSpecificOrderDto
    {
        public GetOrderDetailsDto OrderDetails { get; set; }
        public List<OrderDto> Orders { get; set; }
    }
}
