using ITPLibrary.Api.Core.Dtos.ShoppingCart;
using ITPLibrary.Api.Core.Services.ShoppingCartService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ITPLibrary.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/shopping-cart")]
    [Produces("application/json")]

    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<List<GetShoppingCartDto>>> GetShoppingCart()
        {
            var response = await _shoppingCartService.GetShoppingCart();
            return Ok(Json(response));
        }

        [HttpPost("add-to-cart")]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<List<GetShoppingCartDto>>> AddToShoppingCart(AddShoppingCartDto newItem)
        {
            try
            {
                var response = await _shoppingCartService.AddToShoppingCart(newItem);
                return Ok(Json(response));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("remove-from-cart")]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<List<GetShoppingCartDto>>> RemoveItemFromShoppingCart(int productId)
        {
            try
            {
                var response = await _shoppingCartService.RemoveItemFromShoppingCart(productId);
                return Ok(Json(response));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("delete-cart")]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<string>> RemoveAllItemsFromCart(int userId)
        {
            try
            {
                var response = await _shoppingCartService.RemoveAllItemsFromCart(userId);
                return Ok(Json(response));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
