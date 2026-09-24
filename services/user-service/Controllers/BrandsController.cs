using Microsoft.AspNetCore.Mvc;
using user_service.DTOs.Request;
using user_service.DTOs.Response;
using user_service.Services;

namespace user_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandResponseDto>>> GetAll()
        {
            var brands = await _brandService.GetAllAsync();
            return Ok(brands);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<BrandResponseDto>> GetById(long id)
        {
            var brand = await _brandService.GetByIdAsync(id);
            if (brand == null)
                return NotFound(new { message = $"Brand with ID {id} not found." });

            return Ok(brand);
        }

        [HttpPost]
        public async Task<ActionResult<BrandResponseDto>> Create([FromBody] CreateBrandDto dto)
        {
            try
            {
                var created = await _brandService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<BrandResponseDto>> Update(long id, [FromBody] UpdateBrandDto dto)
        {
            var updated = await _brandService.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound(new { message = $"Brand with ID {id} not found." });

            return Ok(updated);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var success = await _brandService.DeleteAsync(id);
            if (!success)
                return NotFound(new { message = $"Brand with ID {id} not found." });

            return NoContent();
        }
    }
}
