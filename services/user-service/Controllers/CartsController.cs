using Microsoft.AspNetCore.Mvc;
using user_service.DTOs.Request;
using user_service.DTOs.Response;
using user_service.Services;

namespace user_service.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET /api/cart/customer/{customerId}
        [HttpGet("customer/{customerId:long}")]
        public async Task<ActionResult<CartResponseDto>> GetCart(long customerId)
        {
            var cart = await _cartService.GetCartByCustomerIdAsync(customerId);
            return Ok(cart);
        }

        // POST /api/cart/items
        [HttpPost("items")]
        public async Task<ActionResult<CartResponseDto>> AddItem([FromBody] AddCartItemDto dto)
        {
            try
            {
                var cart = await _cartService.AddItemToCartAsync(dto);
                return Ok(cart);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // PUT /api/cart/items/{id}
        [HttpPut("items/{id:long}")]
        public async Task<ActionResult<CartResponseDto>> UpdateQuantity(long id, [FromBody] UpdateCartItemQuantityDto dto)
        {
            try
            {
                var cart = await _cartService.UpdateItemQuantityAsync(id, dto.Quantity);
                return Ok(cart);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // DELETE /api/cart/items/{id}
        [HttpDelete("items/{id:long}")]
        public async Task<IActionResult> RemoveItem(long id)
        {
            var success = await _cartService.RemoveItemFromCartAsync(id);
            if (!success)
                return NotFound(new { message = $"Cart item with ID {id} not found." });

            return NoContent();
        }

        // DELETE /api/cart/customer/{customerId}/clear
        [HttpDelete("customer/{customerId:long}/clear")]
        public async Task<IActionResult> ClearCart(long customerId)
        {
            await _cartService.ClearCartByCustomerIdAsync(customerId);
            return NoContent();
        }
    }
}
