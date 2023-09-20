using Dapper;
using ITPLibrary.Api.Data.Shared.Entities;
using ITPLibrary.Api.Data.Shared.IRepository;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Security.Claims;

namespace ITPLibrary.Api.Data.SQL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConnection _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public OrderRepository(IDbConnection connectionString, IHttpContextAccessor httpContextAccessor, IShoppingCartRepository shoppingCartRepository)
        {
            _connectionString = connectionString;
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
                throw new Exception("User not authorized");
            }

            if (currentShoppingCart == null || currentShoppingCart!.Count <= 0)
            {
                throw new Exception("Cart is empty. Order can not be created.");
            }

            foreach (var product in currentShoppingCart)
            {
                cartTotalPrice += product.CartPrice * product.Count;
            }
            newOrderDetails.OrderPrice = cartTotalPrice;

            await _connectionString.ExecuteAsync(
                @"INSERT INTO OrderDetails (UserId, OrderPrice, PhoneNumber, StreetAddress, City, State, PostalCode, FullName, OrderDate, OrderStatus)
                VALUES (@UserId, @OrderPrice, @PhoneNumber, @StreetAddress, @City, @State, @PostalCode, @FullName, @OrderDate, @OrderStatus);
                SELECT CAST(SCOPE_IDENTITY() AS int)",
                newOrderDetails
            );

            var queryForOrderDetailsId = @"
                SELECT MAX(Id) 
                FROM OrderDetails 
                WHERE UserId = @UserId";

            var currentOrderDetailsId = await _connectionString.ExecuteScalarAsync<int?>(
                queryForOrderDetailsId, new { UserId = currentUserId }
            );

            foreach (var product in currentShoppingCart)
            {
                await _connectionString.ExecuteAsync(
                    @"INSERT INTO Orders (OrderDetailsId, BookId, Count, ProductPrice)
                    VALUES (@OrderDetailsId, @BookId, @Count, @ProductPrice)",
                    new
                    {
                        OrderDetailsId = currentOrderDetailsId.Value,
                        BookId = product.BookId,
                        Count = product.Count,
                        ProductPrice = product.CartPrice
                    }
                );
            }

            await _shoppingCartRepository.RemoveAllItemsFromCart(currentUserId);

            newOrderDetails.Id = currentOrderDetailsId.Value;

            return newOrderDetails;
        }

        public async Task<OrderDetails> GetUserOrder(int orderDetailsId)
        {
            var currentUserId = GetUserId();

            var userOrderDetails = await _connectionString.QueryFirstOrDefaultAsync<OrderDetails>(
                @"SELECT FROM OrderDetails
                WHERE Id = @Id AND UserId = @UserId",
                new { Id = orderDetailsId, UserId = currentUserId }
            );

            if (userOrderDetails == null )
            {
                throw new Exception("Order doesn't exist.");
            }

            return userOrderDetails;
        }

        public async Task<List<Order>> GetUserOrderProducts(int orderDetailsId)
        {
            var currentUserId = GetUserId();

            var userOrderProducts = (await _connectionString.QueryAsync<Order>(
                @"SELECT * FROM Orders
                WHERE OrderDetailsId = @OrderDetailsId",
                new { OrderDetailsId = orderDetailsId }
            )).ToList();

            if (userOrderProducts.Count <= 0 || userOrderProducts == null)
            {
                throw new Exception("No products in this order.");
            }

            return userOrderProducts;
        }
    }
}
