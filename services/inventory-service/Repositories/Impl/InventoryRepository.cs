using Microsoft.EntityFrameworkCore;
using inventory_service.Data;
using inventory_service.Models;

namespace inventory_service.Repositories.Impl
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly InventoryDbContext _context;

        public InventoryRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<Inventory?> GetByVariantIdAsync(long variantId)
        {
            return await _context.Inventories.FindAsync(variantId);
        }

        public async Task<IEnumerable<Inventory>> GetAllAsync()
        {
            return await _context.Inventories.ToListAsync();
        }

        public async Task<Inventory> AddOrUpdateInventoryAsync(Inventory inventory)
        {
            var existing = await _context.Inventories.FindAsync(inventory.ProductVariantId);
            if (existing == null)
            {
                await _context.Inventories.AddAsync(inventory);
            }
            else
            {
                existing.QuantityOnHand = inventory.QuantityOnHand;
                existing.QuantityReserved = inventory.QuantityReserved;
                existing.ReorderLevel = inventory.ReorderLevel;
                existing.UpdatedAt = DateTime.UtcNow;
                _context.Inventories.Update(existing);
            }
            await _context.SaveChangesAsync();
            return inventory;
        }

        public async Task<StockReservation> CreateReservationAsync(StockReservation reservation)
        {
            await _context.StockReservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }

        public async Task<IEnumerable<StockReservation>> GetReservationsByOrderIdAsync(long orderId)
        {
            return await _context.StockReservations
                .Where(r => r.OrderId == orderId)
                .ToListAsync();
        }

        public async Task UpdateReservationAsync(StockReservation reservation)
        {
            _context.StockReservations.Update(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task AddTransactionAsync(StockTransaction transaction)
        {
            await _context.StockTransactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<StockTransaction>> GetTransactionsByVariantIdAsync(long variantId)
        {
            return await _context.StockTransactions
                .Where(t => t.ProductVariantId == variantId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
    }
}