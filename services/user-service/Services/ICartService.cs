using user_service.DTOs.Request;
using user_service.DTOs.Response;

namespace user_service.Services
{
    public interface ICartService
    {
        Task<CartResponseDto> GetCartByCustomerIdAsync(long customerId);
        Task<CartResponseDto> AddItemToCartAsync(AddCartItemDto dto);
        Task<CartResponseDto> UpdateItemQuantityAsync(long cartItemId, decimal newQuantity);
        Task<bool> RemoveItemFromCartAsync(long cartItemId);
        Task<bool> ClearCartByCustomerIdAsync(long customerId);
    }
}
