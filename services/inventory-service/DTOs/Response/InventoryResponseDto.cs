namespace inventory_service.DTOs.Response
{
    public class InventoryResponseDto
    {
        public long ProductVariantId { get; set; }
        public decimal QuantityOnHand { get; set; }
        public decimal QuantityReserved { get; set; }
        public decimal AvailableStock { get; set; }
        public decimal? ReorderLevel { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class StockReservationResponseDto
    {
        public long Id { get; set; }
        public long ProductVariantId { get; set; }
        public long OrderId { get; set; }
        public decimal Quantity { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }
    }
}