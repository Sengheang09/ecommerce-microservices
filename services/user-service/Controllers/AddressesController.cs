using Microsoft.AspNetCore.Mvc;
using user_service.DTOs.Request;
using user_service.DTOs.Response;
using user_service.Services;

namespace user_service.Controllers
{
    [ApiController]
    [Route("api")]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        // GET /api/customers/{customerId}/addresses
        [HttpGet("customers/{customerId:long}/addresses")]
        public async Task<ActionResult<IEnumerable<AddressResponseDto>>> GetByCustomer(long customerId)
        {
            var addresses = await _addressService.GetByCustomerIdAsync(customerId);
            return Ok(addresses);
        }

        // GET /api/addresses/{id}
        [HttpGet("addresses/{id:long}")]
        public async Task<ActionResult<AddressResponseDto>> GetById(long id)
        {
            var address = await _addressService.GetByIdAsync(id);
            if (address == null)
                return NotFound(new { message = $"Address with ID {id} not found." });

            return Ok(address);
        }

        // POST /api/customers/{customerId}/addresses
        [HttpPost("customers/{customerId:long}/addresses")]
        public async Task<ActionResult<AddressResponseDto>> Create(long customerId, [FromBody] CreateAddressDto dto)
        {
            try
            {
                var created = await _addressService.CreateAsync(customerId, dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // PUT /api/addresses/{id}
        [HttpPut("addresses/{id:long}")]
        public async Task<ActionResult<AddressResponseDto>> Update(long id, [FromBody] UpdateAddressDto dto)
        {
            var updated = await _addressService.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound(new { message = $"Address with ID {id} not found." });

            return Ok(updated);
        }

        // PUT /api/customers/{customerId}/addresses/{id}/default
        [HttpPut("customers/{customerId:long}/addresses/{id:long}/default")]
        public async Task<IActionResult> SetDefault(long customerId, long id)
        {
            var success = await _addressService.SetDefaultAddressAsync(customerId, id);
            if (!success)
                return NotFound(new { message = $"Address with ID {id} for customer {customerId} not found." });

            return NoContent();
        }

        // DELETE /api/addresses/{id}
        [HttpDelete("addresses/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var success = await _addressService.DeleteAsync(id);
            if (!success)
                return NotFound(new { message = $"Address with ID {id} not found." });

            return NoContent();
        }
    }
}
