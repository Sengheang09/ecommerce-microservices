using Microsoft.AspNetCore.Mvc;
using inventory_service.DTOs.Request;
using inventory_service.DTOs.Response;
using inventory_service.Services;

namespace inventory_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _service;

        public InventoryController(IInventoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryResponseDto>>> GetAll()
        {
            var list = await _service.GetAllStockAsync();
            return Ok(list);
        }

        // GET 
        [HttpGet("{variantId:long}")]
        public async Task<ActionResult<InventoryResponseDto>> GetByVariant(long variantId)
        {
            var stock = await _service.GetStockAsync(variantId);
            if (stock == null)
                return NotFound(new { message = $"No inventory record found for variant {variantId}." });

            return Ok(stock);
        }

        [HttpPost("add-stock")]
        public async Task<ActionResult<InventoryResponseDto>> AddStock([FromBody] AddStockDto dto)
        {
            var result = await _service.AddStockAsync(dto);
            return Ok(result);
        }

        [HttpPost("reserve")]
        public async Task<ActionResult<IEnumerable<StockReservationResponseDto>>> Reserve([FromBody] ReserveStockDto dto)
        {
            try
            {
                var result = await _service.ReserveStockAsync(dto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("confirm/{orderId:long}")]
        public async Task<IActionResult> ConfirmDeduction(long orderId)
        {
            var success = await _service.ConfirmStockDeductionAsync(orderId);
            if (!success)
                return NotFound(new { message = $"No active reservations found for order {orderId}." });

            return Ok(new { message = $"Stock successfully deducted for order {orderId}." });
        }

        [HttpPost("release/{orderId:long}")]
        public async Task<IActionResult> Release(long orderId)
        {
            var success = await _service.ReleaseReservationAsync(orderId);
            if (!success)
                return NotFound(new { message = $"No active reservations found for order {orderId}." });

            return Ok(new { message = $"Stock reservation successfully released for order {orderId}." });
        }
    }
}