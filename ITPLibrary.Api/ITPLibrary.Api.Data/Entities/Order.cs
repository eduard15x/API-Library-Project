using System.ComponentModel.DataAnnotations.Schema;

namespace ITPLibrary.Api.Data.Shared.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int OrderDetailsId { get; set; }
        [ForeignKey("OrderDetailsId")]
        public OrderDetails OrderDetails { get; set; }
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }
        public int Count { get; set; }
        public double ProductPrice { get; set; }
    }
}
