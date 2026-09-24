using user_service.Models;

namespace user_service.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetByCustomerIdAsync(long customerId);
        Task<Cart> CreateCartAsync(long customerId);
        Task<CartItem?> GetCartItemByIdAsync(long cartItemId);
        Task<CartItem?> FindCartItemAsync(long cartId, long productVariantId);
        Task<CartItem> AddItemAsync(CartItem item);
        Task<CartItem> UpdateItemAsync(CartItem item);
        Task<bool> RemoveItemAsync(long cartItemId);
        Task<bool> ClearCartAsync(long cartId);
    }
}
