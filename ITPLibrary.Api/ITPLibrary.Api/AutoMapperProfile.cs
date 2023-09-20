using ITPLibrary.Api.Core.Dtos.Book;
using ITPLibrary.Api.Core.Dtos.FavoriteProduct;
using ITPLibrary.Api.Core.Dtos.Order;
using ITPLibrary.Api.Core.Dtos.ShoppingCart;
using ITPLibrary.Api.Core.Dtos.User;
using ITPLibrary.Api.Data.Shared.Entities;

namespace ITPLibrary.Api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Book, GetBookDto>();
            CreateMap<Book, PromotedBookDto>();
            CreateMap<Book, BestBookDto>();
            CreateMap<AddBookDto, Book>();
            CreateMap<UpdateBookDto, Book>();
            CreateMap<UserDto, User>().ReverseMap(); 
            CreateMap<ShoppingCartModel, GetShoppingCartDto>().ReverseMap();
            CreateMap<ShoppingCartModel, AddShoppingCartDto>().ReverseMap();
            CreateMap<OrderDetails, GetOrderDetailsDto>().ReverseMap();
            CreateMap<OrderDetails, AddNewOrderDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<FavoriteProduct, GetFavoriteProductDto>().ReverseMap();
        }
    }
}
