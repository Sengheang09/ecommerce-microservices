using System.ComponentModel.DataAnnotations;

namespace inventory_service.DTOs.Request
{
    public class AddStockDto
    {
        [Required]
        public long ProductVariantId { get; set; }

        [Required]
        [Range(0.01, 1000000, ErrorMessage = "Quantity must be greater than 0")]
        public decimal Quantity { get; set; }

        public string? ReferenceType { get; set; } = "PURCHASE";
        
        public long? ReferenceId { get; set; }

    }
}