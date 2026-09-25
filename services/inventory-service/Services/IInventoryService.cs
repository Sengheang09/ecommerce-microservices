using inventory_service.DTOs.Request;
using inventory_service.DTOs.Response;

namespace inventory_service.Services
{
    public interface IInventoryService
    {
        Task<InventoryResponseDto?> GetStockAsync(long variantId);

        Task<IEnumerable<InventoryResponseDto>> GetAllStockAsync();

        Task<InventoryResponseDto> AddStockAsync(AddStockDto dto);

        Task<IEnumerable<StockReservationResponseDto>> ReserveStockAsync(ReserveStockDto dto);

        Task<bool> ConfirmStockDeductionAsync(long orderId);
        
        Task<bool> ReleaseReservationAsync(long orderId);
    }
}