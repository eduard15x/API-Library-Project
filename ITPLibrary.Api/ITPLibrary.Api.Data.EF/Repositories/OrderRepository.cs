using ITPLibrary.Api.Data.EF.Data;
using ITPLibrary.Api.Data.Shared.Entities;
using ITPLibrary.Api.Data.Shared.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ITPLibrary.Api.Data.EF.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public OrderRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IShoppingCartRepository shoppingCartRepository)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _shoppingCartRepository = shoppingCartRepository;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<OrderDetails> CreateNewOrder(OrderDetails newOrderDetails)
        {
            var currentUserId = GetUserId();
            var currentShoppingCart = await _shoppingCartRepository.GetShoppingCart();
            double cartTotalPrice = 0;

            if (currentUserId != newOrderDetails.UserId)
            {
                throw new Exception($"User not authorized.");
            }

            if (currentShoppingCart.Count <= 0)
            {
                throw new Exception($"Cart is empty. Order can not be created.");
            }
            
            // update cart total price based on number of products present in cart and their count (number of each)
            foreach (var product in currentShoppingCart)
            {
                cartTotalPrice += product.CartPrice * product.Count;
            }
            newOrderDetails.OrderPrice = cartTotalPrice;
            // create first record in database, the new order details row to OrderDetails table
            _context.OrderDetails.Add(newOrderDetails);
            await _context.SaveChangesAsync();

            // add the products ordered in the previous order in Orders table to keep track of them
            // get the last id of orderDetails created for the current user
            var currentUserLastOrderDetailsId = await _context.OrderDetails
                .Where(o => o.UserId == currentUserId)
                .OrderByDescending(o => o.Id) // Assuming 'Id' is the auto-incrementing primary key
                .FirstOrDefaultAsync();

            foreach (var product in currentShoppingCart)
            {
                var newOrderRecord = new Order
                {
                    OrderDetailsId = currentUserLastOrderDetailsId.Id,
                    BookId = product.BookId,
                    Count = product.Count,
                    ProductPrice = product.CartPrice
                };

                _context.Orders.Add(newOrderRecord);
                await _context.SaveChangesAsync();
            }

            await _shoppingCartRepository.RemoveAllItemsFromCart(newOrderDetails.UserId);

            return newOrderDetails;
        }

        public async Task<OrderDetails> GetUserOrder(int orderDetailsId)
        {
            var currentUserId = GetUserId();
            var userOrderDetails = await _context.OrderDetails
                .FirstOrDefaultAsync(o => o.Id == orderDetailsId && o.UserId == currentUserId);

            if (userOrderDetails is null)
            {
                throw new Exception($"No order with number {orderDetailsId} for current user.");
            }

            return userOrderDetails;
        }

        public async Task<List<Order>> GetUserOrderProducts(int orderDetailsId)
        {
            var orderDetails = await GetUserOrder(orderDetailsId);

            return await _context.Orders
                .Where(p => p.OrderDetailsId == orderDetails.Id)
                .ToListAsync();
        }
    }
}
