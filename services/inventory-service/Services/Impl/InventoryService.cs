using inventory_service.DTOs.Request;
using inventory_service.DTOs.Response;
using inventory_service.Models;
using inventory_service.Repositories;

namespace inventory_service.Services.Impl
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _repository;

        public InventoryService(IInventoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<InventoryResponseDto?> GetStockAsync(long variantId)
        {
            var inv = await _repository.GetByVariantIdAsync(variantId);
            return inv == null ? null : MapToDto(inv);
        }

        public async Task<IEnumerable<InventoryResponseDto>> GetAllStockAsync()
        {
            var list = await _repository.GetAllAsync();
            return list.Select(MapToDto);
        }

        public async Task<InventoryResponseDto> AddStockAsync(AddStockDto dto)
        {
            var inv = await _repository.GetByVariantIdAsync(dto.ProductVariantId);
            if (inv == null)
            {
                inv = new Inventory
                {
                    ProductVariantId = dto.ProductVariantId,
                    QuantityOnHand = dto.Quantity,
                    QuantityReserved = 0,
                    UpdatedAt = DateTime.UtcNow
                };
            }
            else
            {
                inv.QuantityOnHand += dto.Quantity;
                inv.UpdatedAt = DateTime.UtcNow;
            }

            await _repository.AddOrUpdateInventoryAsync(inv);

            // Audit Transaction
            await _repository.AddTransactionAsync(new StockTransaction
            {
                ProductVariantId = dto.ProductVariantId,
                TransactionType = "IN",
                Quantity = dto.Quantity,
                ReferenceType = dto.ReferenceType,
                ReferenceId = dto.ReferenceId,
                CreatedAt = DateTime.UtcNow
            });

            return MapToDto(inv);
        }

        public async Task<IEnumerable<StockReservationResponseDto>> ReserveStockAsync(ReserveStockDto dto)
        {
            // ១. ពិនិត្យមើលថាតើស្តុកគ្រប់គ្រាន់សម្រាប់ទំនិញទាំងអស់ឬអត់ (All-or-Nothing)
            foreach (var item in dto.Items)
            {
                var inv = await _repository.GetByVariantIdAsync(item.ProductVariantId);
                if (inv == null || inv.AvailableStock < item.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for Product Variant {item.ProductVariantId}. Available: {inv?.AvailableStock ?? 0}");
                }
            }

            // ២. កក់ស្តុក (Reserve)
            var reservations = new List<StockReservation>();
            foreach (var item in dto.Items)
            {
                var inv = await _repository.GetByVariantIdAsync(item.ProductVariantId);
                inv!.QuantityReserved += item.Quantity;
                await _repository.AddOrUpdateInventoryAsync(inv);

                var reservation = new StockReservation
                {
                    ProductVariantId = item.ProductVariantId,
                    OrderId = dto.OrderId,
                    Quantity = item.Quantity,
                    Status = "RESERVED",
                    ExpiresAt = DateTime.UtcNow.AddMinutes(dto.ExpirationMinutes)
                };

                var created = await _repository.CreateReservationAsync(reservation);
                reservations.Add(created);
            }

            return reservations.Select(r => new StockReservationResponseDto
            {
                Id = r.Id,
                ProductVariantId = r.ProductVariantId,
                OrderId = r.OrderId,
                Quantity = r.Quantity,
                Status = r.Status,
                ExpiresAt = r.ExpiresAt
            });
        }

        public async Task<bool> ConfirmStockDeductionAsync(long orderId)
        {
            var reservations = await _repository.GetReservationsByOrderIdAsync(orderId);
            if (!reservations.Any()) return false;

            foreach (var res in reservations.Where(r => r.Status == "RESERVED"))
            {
                var inv = await _repository.GetByVariantIdAsync(res.ProductVariantId);
                if (inv != null)
                {
                    inv.QuantityOnHand -= res.Quantity;
                    inv.QuantityReserved -= res.Quantity;
                    if (inv.QuantityOnHand < 0) inv.QuantityOnHand = 0;
                    if (inv.QuantityReserved < 0) inv.QuantityReserved = 0;
                    await _repository.AddOrUpdateInventoryAsync(inv);

                    // Audit Transaction OUT
                    await _repository.AddTransactionAsync(new StockTransaction
                    {
                        ProductVariantId = res.ProductVariantId,
                        TransactionType = "OUT",
                        Quantity = res.Quantity,
                        ReferenceType = "ORDER",
                        ReferenceId = orderId,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                res.Status = "CONFIRMED";
                await _repository.UpdateReservationAsync(res);
            }

            return true;
        }

        public async Task<bool> ReleaseReservationAsync(long orderId)
        {
            var reservations = await _repository.GetReservationsByOrderIdAsync(orderId);
            if (!reservations.Any()) return false;

            foreach (var res in reservations.Where(r => r.Status == "RESERVED"))
            {
                var inv = await _repository.GetByVariantIdAsync(res.ProductVariantId);
                if (inv != null)
                {
                    inv.QuantityReserved -= res.Quantity;
                    if (inv.QuantityReserved < 0) inv.QuantityReserved = 0;
                    await _repository.AddOrUpdateInventoryAsync(inv);
                }

                res.Status = "RELEASED";
                await _repository.UpdateReservationAsync(res);
            }

            return true;
        }

        private static InventoryResponseDto MapToDto(Inventory i) => new()
        {
            ProductVariantId = i.ProductVariantId,
            QuantityOnHand = i.QuantityOnHand,
            QuantityReserved = i.QuantityReserved,
            AvailableStock = i.AvailableStock,
            ReorderLevel = i.ReorderLevel,
            UpdatedAt = i.UpdatedAt
        };
    }
}