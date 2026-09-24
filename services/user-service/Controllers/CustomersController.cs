using Microsoft.AspNetCore.Mvc;
using user_service.DTOs.Request;
using user_service.DTOs.Response;
using user_service.Services;

namespace user_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<CustomerResponseDto>> GetById(long id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
                return NotFound(new { message = $"Customer profile with ID {id} not found." });

            return Ok(customer);
        }

        [HttpGet("user/{userId:long}")]
        public async Task<ActionResult<CustomerResponseDto>> GetByUserId(long userId)
        {
            var customer = await _customerService.GetByUserIdAsync(userId);
            if (customer == null)
                return NotFound(new { message = $"Customer profile for UserId {userId} not found." });

            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerResponseDto>> Create([FromBody] CreateCustomerDto dto)
        {
            try
            {
                var created = await _customerService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<CustomerResponseDto>> Update(long id, [FromBody] UpdateCustomerDto dto)
        {
            var updated = await _customerService.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound(new { message = $"Customer profile with ID {id} not found." });

            return Ok(updated);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var success = await _customerService.DeleteAsync(id);
            if (!success)
                return NotFound(new { message = $"Customer profile with ID {id} not found." });

            return NoContent();
        }
    }
}