using Microsoft.AspNetCore.Mvc;
using user_service.DTOs.Request;
using user_service.DTOs.Response;
using user_service.Services;

namespace user_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET /api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        // GET /api/orders/{id}
        [HttpGet("{id:long}")]
        public async Task<ActionResult<OrderResponseDto>> GetById(long id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound(new { message = $"Order with ID {id} not found." });

            return Ok(order);
        }

        // GET /api/orders/customer/{customerId}
        [HttpGet("customer/{customerId:long}")]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetByCustomer(long customerId)
        {
            var orders = await _orderService.GetByCustomerIdAsync(customerId);
            return Ok(orders);
        }

        // POST /api/orders/checkout
        [HttpPost("checkout")]
        public async Task<ActionResult<OrderResponseDto>> Checkout([FromBody] CheckoutDto dto)
        {
            try
            {
                var order = await _orderService.CheckoutAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT /api/orders/{id}/status
        [HttpPut("{id:long}/status")]
        public async Task<ActionResult<OrderResponseDto>> UpdateStatus(long id, [FromBody] UpdateOrderStatusDto dto)
        {
            var order = await _orderService.UpdateStatusAsync(id, dto.Status);
            if (order == null)
                return NotFound(new { message = $"Order with ID {id} not found." });

            return Ok(order);
        }
    }
}
