using ITPLibrary.Api.Data.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITPLibrary.Api.Data.EF.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ShoppingCartModel> ShoppingCarts { get; set;}
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<FavoriteProduct> FavoriteProducts { get; set;}
    }
}
