namespace ITPLibrary.Api.Core.Dtos.Checkout
{
    public class PaymentDto
    {
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string Cvc { get; set; }
    }
}
