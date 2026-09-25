using System.ComponentModel.DataAnnotations;

namespace inventory_service.DTOs.Request
{
    public class ReserveStockItemDto
    {
        [Required]
        public long ProductVariantId { get; set; }

        [Required]
        [Range(0.01, 1000000, ErrorMessage = "Quantity must be greater than 0")]
        public decimal Quantity { get; set; }
    }

    public class ReserveStockDto
    {
        [Required]
        public long OrderId { get; set; }

        [Required]
        public List<ReserveStockItemDto> Items { get; set; } = new();

        public int ExpirationMinutes { get; set; } = 15;
    }
}