using inventory_service.Models;

namespace inventory_service.Repositories
{
    public interface IInventoryRepository
    {
        Task<Inventory?> GetByVariantIdAsync(long variantId);

        Task<IEnumerable<Inventory>> GetAllAsync();

        Task<Inventory> AddOrUpdateInventoryAsync(Inventory inventory);

        Task<StockReservation> CreateReservationAsync(StockReservation reservation);

        Task<IEnumerable<StockReservation>> GetReservationsByOrderIdAsync(long orderId);

        Task UpdateReservationAsync(StockReservation reservation);

        Task AddTransactionAsync(StockTransaction transaction);
        
        Task<IEnumerable<StockTransaction>> GetTransactionsByVariantIdAsync(long variantId);
    }
}