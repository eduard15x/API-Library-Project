using System.ComponentModel.DataAnnotations;

namespace ITPLibrary.Api.Core.Dtos.Order
{
    public class AddNewOrderDto
    {
        public int UserId { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string StreetAddress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string FullName { get; set; }
    }
}
