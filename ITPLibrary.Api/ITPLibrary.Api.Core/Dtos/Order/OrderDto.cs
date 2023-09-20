namespace ITPLibrary.Api.Core.Dtos.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Count { get; set; }
        public double ProductPrice { get; set; }
    }
}
