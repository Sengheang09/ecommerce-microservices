using user_service.DTOs.Request;
using user_service.DTOs.Response;
using user_service.Models;
using user_service.Repositories;

namespace user_service.Services.Impl
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAddressRepository _addressRepository;

        public OrderService(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            ICustomerRepository customerRepository,
            IAddressRepository addressRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(MapToDto);
        }

        public async Task<IEnumerable<OrderResponseDto>> GetByCustomerIdAsync(long customerId)
        {
            var orders = await _orderRepository.GetByCustomerIdAsync(customerId);
            return orders.Select(MapToDto);
        }

        public async Task<OrderResponseDto?> GetByIdAsync(long id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return order == null ? null : MapToDto(order);
        }

        public async Task<OrderResponseDto> CheckoutAsync(CheckoutDto dto)
        {
            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {dto.CustomerId} not found.");
            }

            var address = await _addressRepository.GetByIdAsync(dto.AddressId);
            if (address == null || address.CustomerProfileId != dto.CustomerId)
            {
                throw new ArgumentException($"Valid shipping address for customer {dto.CustomerId} not found.");
            }

            var cart = await _cartRepository.GetByCustomerIdAsync(dto.CustomerId);
            if (cart == null || !cart.Items.Any())
            {
                throw new InvalidOperationException("Cannot checkout with an empty cart.");
            }

            decimal subtotal = 0;
            var orderItems = new List<OrderItem>();

            foreach (var item in cart.Items)
            {
                var variant = item.ProductVariant;
                var unitPrice = variant?.Price ?? 0;
                var itemSubtotal = unitPrice * item.Quantity;
                subtotal += itemSubtotal;

                var productName = variant?.Product?.Name ?? "Product";
                if (!string.IsNullOrEmpty(variant?.Sku))
                {
                    productName += $" ({variant.Sku})";
                }

                orderItems.Add(new OrderItem
                {
                    ProductVariantId = item.ProductVariantId,
                    ProductNameSnapshot = productName,
                    UnitPrice = unitPrice,
                    Quantity = item.Quantity,
                    Subtotal = itemSubtotal
                });
            }

            decimal total = subtotal - dto.Discount + dto.ShippingFee;
            if (total < 0) total = 0;

            var orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{new Random().Next(100, 999)}";

            var order = new Order
            {
                OrderNumber = orderNumber,
                CustomerId = dto.CustomerId,
                AddressId = dto.AddressId,
                Status = "PENDING",
                Subtotal = subtotal,
                Discount = dto.Discount,
                ShippingFee = dto.ShippingFee,
                Total = total,
                CreatedAt = DateTime.UtcNow,
                Items = orderItems
            };

            var createdOrder = await _orderRepository.CreateAsync(order);

            // Add Initial Status History
            await _orderRepository.AddStatusHistoryAsync(new OrderStatusHistory
            {
                OrderId = createdOrder.Id,
                OldStatus = null,
                NewStatus = "PENDING",
                ChangedAt = DateTime.UtcNow
            });

            // Clear Cart after successful checkout
            await _cartRepository.ClearCartAsync(cart.Id);

            var reloaded = await _orderRepository.GetByIdAsync(createdOrder.Id);
            return MapToDto(reloaded ?? createdOrder);
        }

        public async Task<OrderResponseDto?> UpdateStatusAsync(long id, string newStatus)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return null;

            var oldStatus = order.Status;
            order.Status = newStatus.ToUpper();

            await _orderRepository.UpdateAsync(order);

            await _orderRepository.AddStatusHistoryAsync(new OrderStatusHistory
            {
                OrderId = order.Id,
                OldStatus = oldStatus,
                NewStatus = order.Status,
                ChangedAt = DateTime.UtcNow
            });

            var reloaded = await _orderRepository.GetByIdAsync(id);
            return MapToDto(reloaded ?? order);
        }

        private static OrderResponseDto MapToDto(Order o)
        {
            return new OrderResponseDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                CustomerId = o.CustomerId,
                CustomerName = o.Customer != null ? $"{o.Customer.FirstName} {o.Customer.LastName}".Trim() : null,
                AddressId = o.AddressId,
                ShippingAddressLine = o.ShippingAddress != null 
                    ? $"{o.ShippingAddress.AddressLine}, {o.ShippingAddress.City}".Trim().TrimEnd(',') 
                    : null,
                Status = o.Status,
                Subtotal = o.Subtotal,
                Discount = o.Discount,
                ShippingFee = o.ShippingFee,
                Total = o.Total,
                CreatedAt = o.CreatedAt,
                Items = o.Items.Select(i => new OrderItemResponseDto
                {
                    Id = i.Id,
                    ProductVariantId = i.ProductVariantId,
                    ProductNameSnapshot = i.ProductNameSnapshot,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    Subtotal = i.Subtotal
                }).ToList(),
                StatusHistories = o.StatusHistories.Select(h => new OrderStatusHistoryResponseDto
                {
                    Id = h.Id,
                    OldStatus = h.OldStatus,
                    NewStatus = h.NewStatus,
                    ChangedAt = h.ChangedAt
                }).OrderByDescending(h => h.ChangedAt).ToList()
            };
        }
    }
}
