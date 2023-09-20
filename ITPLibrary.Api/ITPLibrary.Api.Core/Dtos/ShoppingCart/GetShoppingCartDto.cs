namespace ITPLibrary.Api.Core.Dtos.ShoppingCart
{
    public class GetShoppingCartDto
    {
        public int BookId { get; set; }
        public int Count { get; set; }
        public double CartPrice { get; set; }
    }
}
