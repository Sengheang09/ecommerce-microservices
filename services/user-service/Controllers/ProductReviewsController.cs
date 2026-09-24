using Microsoft.AspNetCore.Mvc;
using user_service.DTOs.Request;
using user_service.DTOs.Response;
using user_service.Services;

namespace user_service.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProductReviewsController : ControllerBase
    {
        private readonly IProductReviewService _reviewService;

        public ProductReviewsController(IProductReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // GET /api/products/{productId}/reviews
        [HttpGet("products/{productId:long}/reviews")]
        public async Task<ActionResult<IEnumerable<ProductReviewResponseDto>>> GetByProduct(long productId)
        {
            var reviews = await _reviewService.GetByProductIdAsync(productId);
            return Ok(reviews);
        }

        // GET /api/reviews/{id}
        [HttpGet("reviews/{id:long}")]
        public async Task<ActionResult<ProductReviewResponseDto>> GetById(long id)
        {
            var review = await _reviewService.GetByIdAsync(id);
            if (review == null)
                return NotFound(new { message = $"Review with ID {id} not found." });

            return Ok(review);
        }

        // POST /api/products/{productId}/reviews
        [HttpPost("products/{productId:long}/reviews")]
        public async Task<ActionResult<ProductReviewResponseDto>> Create(long productId, [FromBody] CreateProductReviewDto dto)
        {
            try
            {
                var created = await _reviewService.CreateAsync(productId, dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // DELETE /api/reviews/{id}
        [HttpDelete("reviews/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var success = await _reviewService.DeleteAsync(id);
            if (!success)
                return NotFound(new { message = $"Review with ID {id} not found." });

            return NoContent();
        }
    }
}
