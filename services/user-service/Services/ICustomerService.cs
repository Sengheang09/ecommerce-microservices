using user_service.DTOs.Request;
using user_service.DTOs.Response;

namespace user_service.Services
{
    public interface ICustomerService
    {
        Task<CustomerResponseDto?> GetByIdAsync(long id);
        Task<CustomerResponseDto?> GetByUserIdAsync(long userId);
        Task<CustomerResponseDto> CreateAsync(CreateCustomerDto dto);
        Task<CustomerResponseDto?> UpdateAsync(long id, UpdateCustomerDto dto);
        Task<bool> DeleteAsync(long id);
    }
}