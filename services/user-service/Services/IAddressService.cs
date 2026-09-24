using user_service.DTOs.Request;
using user_service.DTOs.Response;

namespace user_service.Services
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressResponseDto>> GetByCustomerIdAsync(long customerId);
        Task<AddressResponseDto?> GetByIdAsync(long id);
        Task<AddressResponseDto> CreateAsync(long customerId, CreateAddressDto dto);
        Task<AddressResponseDto?> UpdateAsync(long id, UpdateAddressDto dto);
        Task<bool> DeleteAsync(long id);
        Task<bool> SetDefaultAddressAsync(long customerId, long addressId);
    }
}
