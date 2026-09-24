using user_service.DTOs.Request;
using user_service.DTOs.Response;

namespace user_service.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseDto>> GetAllAsync();
        Task<IEnumerable<OrderResponseDto>> GetByCustomerIdAsync(long customerId);
        Task<OrderResponseDto?> GetByIdAsync(long id);
        Task<OrderResponseDto> CheckoutAsync(CheckoutDto dto);
        Task<OrderResponseDto?> UpdateStatusAsync(long id, string newStatus);
    }
}
