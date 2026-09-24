using Microsoft.AspNetCore.Mvc;
using user_service.DTOs.Request;
using user_service.DTOs.Response;
using user_service.Services;

namespace user_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAll(
            [FromQuery] string? search,
            [FromQuery] long? categoryId,
            [FromQuery] long? brandId)
        {
            var products = await _productService.GetAllAsync(search, categoryId, brandId);
            return Ok(products);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ProductResponseDto>> GetById(long id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound(new { message = $"Product with ID {id} not found." });

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> Create([FromBody] CreateProductDto dto)
        {
            try
            {
                var created = await _productService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<ProductResponseDto>> Update(long id, [FromBody] UpdateProductDto dto)
        {
            var updated = await _productService.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound(new { message = $"Product with ID {id} not found." });

            return Ok(updated);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var success = await _productService.DeleteAsync(id);
            if (!success)
                return NotFound(new { message = $"Product with ID {id} not found." });

            return NoContent();
        }
    }
}