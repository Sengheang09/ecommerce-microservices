using Microsoft.EntityFrameworkCore;
using user_service.Data;
using user_service.Models;

namespace user_service.Repositories.Impl
{
    public class CartRepository : ICartRepository
    {
        private readonly UserDbContext _context;

        public CartRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetByCustomerIdAsync(long customerId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.ProductVariant)
                        .ThenInclude(pv => pv!.Product)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public async Task<Cart> CreateCartAsync(long customerId)
        {
            var cart = new Cart
            {
                CustomerId = customerId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<CartItem?> GetCartItemByIdAsync(long cartItemId)
        {
            return await _context.CartItems
                .Include(i => i.Cart)
                .Include(i => i.ProductVariant)
                    .ThenInclude(pv => pv!.Product)
                .FirstOrDefaultAsync(i => i.Id == cartItemId);
        }

        public async Task<CartItem?> FindCartItemAsync(long cartId, long productVariantId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(i => i.CartId == cartId && i.ProductVariantId == productVariantId);
        }

        public async Task<CartItem> AddItemAsync(CartItem item)
        {
            await _context.CartItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<CartItem> UpdateItemAsync(CartItem item)
        {
            _context.CartItems.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> RemoveItemAsync(long cartItemId)
        {
            var item = await _context.CartItems.FindAsync(cartItemId);
            if (item == null) return false;

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCartAsync(long cartId)
        {
            var items = await _context.CartItems.Where(i => i.CartId == cartId).ToListAsync();
            if (!items.Any()) return true;

            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
