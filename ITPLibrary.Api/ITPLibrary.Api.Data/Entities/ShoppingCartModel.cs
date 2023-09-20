﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ITPLibrary.Api.Data.Shared.Entities
{
    public class ShoppingCartModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }
        public int Count { get; set; }
        public double CartPrice { get; set; }
    }
}
