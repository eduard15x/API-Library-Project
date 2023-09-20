using Azure;
using ITPLibrary.Api.Core.Dtos.Order;
using ITPLibrary.Api.Core.Services.OrderService;
using ITPLibrary.Api.Data.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ITPLibrary.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/order")]
    [Produces("application/json")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("orders")]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<UserSpecificOrderDto>> GetUserOrders(int orderDetailsId)
        {
            try
            {
                var response = await _orderService.GetUserOrder(orderDetailsId);
                return Ok(Json(response));
            }
            catch (Exception ex)
            {
                return NotFound(Json(ex.Message));
            }
        }

        [HttpPost("create-new-order")]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<UserSpecificOrderDto>> CreateNewOrder(AddNewOrderDto newOrderDetails)
        {
            try
            {
                var response = await _orderService.CreateNewOrder(newOrderDetails);
                return Ok(Json(response));
            }
            catch (Exception ex)
            {
                return NotFound(Json(ex.Message));
            }
        }
    }
}
