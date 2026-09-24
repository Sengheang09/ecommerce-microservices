using user_service.DTOs.Request;
using user_service.DTOs.Response;
using user_service.Models;
using user_service.Repositories;

namespace user_service.Services.Impl
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICustomerRepository _customerRepository;

        public CartService(ICartRepository cartRepository, ICustomerRepository customerRepository)
        {
            _cartRepository = cartRepository;
            _customerRepository = customerRepository;
        }

        public async Task<CartResponseDto> GetCartByCustomerIdAsync(long customerId)
        {
            var cart = await _cartRepository.GetByCustomerIdAsync(customerId);
            if (cart == null)
            {
                cart = await _cartRepository.CreateCartAsync(customerId);
            }
            return MapToDto(cart);
        }

        public async Task<CartResponseDto> AddItemToCartAsync(AddCartItemDto dto)
        {
            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {dto.CustomerId} not found.");
            }

            var cart = await _cartRepository.GetByCustomerIdAsync(dto.CustomerId);
            if (cart == null)
            {
                cart = await _cartRepository.CreateCartAsync(dto.CustomerId);
            }

            var existingItem = await _cartRepository.FindCartItemAsync(cart.Id, dto.ProductVariantId);
            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
                await _cartRepository.UpdateItemAsync(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductVariantId = dto.ProductVariantId,
                    Quantity = dto.Quantity
                };
                await _cartRepository.AddItemAsync(newItem);
            }

            var reloadedCart = await _cartRepository.GetByCustomerIdAsync(dto.CustomerId);
            return MapToDto(reloadedCart ?? cart);
        }

        public async Task<CartResponseDto> UpdateItemQuantityAsync(long cartItemId, decimal newQuantity)
        {
            var item = await _cartRepository.GetCartItemByIdAsync(cartItemId);
            if (item == null)
            {
                throw new KeyNotFoundException($"Cart item with ID {cartItemId} not found.");
            }

            if (newQuantity <= 0)
            {
                await _cartRepository.RemoveItemAsync(cartItemId);
            }
            else
            {
                item.Quantity = newQuantity;
                await _cartRepository.UpdateItemAsync(item);
            }

            var reloadedCart = await _cartRepository.GetByCustomerIdAsync(item.Cart!.CustomerId);
            return MapToDto(reloadedCart!);
        }

        public async Task<bool> RemoveItemFromCartAsync(long cartItemId)
        {
            return await _cartRepository.RemoveItemAsync(cartItemId);
        }

        public async Task<bool> ClearCartByCustomerIdAsync(long customerId)
        {
            var cart = await _cartRepository.GetByCustomerIdAsync(customerId);
            if (cart == null) return true;

            return await _cartRepository.ClearCartAsync(cart.Id);
        }

        private static CartResponseDto MapToDto(Cart cart)
        {
            return new CartResponseDto
            {
                Id = cart.Id,
                CustomerId = cart.CustomerId,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                Items = cart.Items.Select(i => new CartItemResponseDto
                {
                    Id = i.Id,
                    ProductVariantId = i.ProductVariantId,
                    Sku = i.ProductVariant?.Sku ?? string.Empty,
                    ProductName = i.ProductVariant?.Product?.Name ?? string.Empty,
                    UnitPrice = i.ProductVariant?.Price ?? 0,
                    Quantity = i.Quantity
                }).ToList()
            };
        }
    }
}
