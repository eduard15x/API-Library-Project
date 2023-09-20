using ITPLibrary.Api.Core.Dtos.FavoriteProduct;
using ITPLibrary.Api.Core.Services.FavoriteProductService;
using ITPLibrary.Api.Data.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ITPLibrary.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/favorites")]
    [Produces("application/json")]
    public class FavoriteProductController : Controller
    {
        private readonly IFavoriteProductService _favoriteProductService;

        public FavoriteProductController(IFavoriteProductService favoriteProductService)
        {
            _favoriteProductService = favoriteProductService;
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<List<GetFavoriteProductDto>>> GetFavoriteProducts(int userId)
        {
            try
            {
                var response = await _favoriteProductService.GetFavoriteProducts(userId);
                return Ok(Json(response));
            }
            catch (Exception ex)
            {
                return NotFound(Json(ex.Message));
            }
        }

        [HttpPost("add-to-favorite")]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<string>> AddProductToFavorite(int productId, int userId)
        {
            try
            {
                var response = await _favoriteProductService.AddProductToFavorite(productId, userId);
                return Ok(Json(response));
            }
            catch (Exception ex)
            {
                return NotFound(Json(ex.Message));
            }
        }

        [HttpDelete("remove-from-favorite")]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<string>> DeleteProductFromFavorites(int productId, int userId)
        {
            try
            {
                var response = await _favoriteProductService.DeleteProductFromFavorites(productId, userId);
                return Ok(Json(response));
            }
            catch (Exception ex)
            {
                return NotFound(Json(ex.Message));
            }
        }
    }
}
