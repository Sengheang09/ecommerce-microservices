using System.ComponentModel.DataAnnotations;

namespace user_service.DTOs.Request
{
    public class AddCartItemDto
    {
        [Required(ErrorMessage = "CustomerId is required")]
        public long CustomerId { get; set; }

        [Required(ErrorMessage = "ProductVariantId is required")]
        public long ProductVariantId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(0.01, 10000, ErrorMessage = "Quantity must be greater than 0")]
        public decimal Quantity { get; set; }
    }

    public class UpdateCartItemQuantityDto
    {
        [Required(ErrorMessage = "Quantity is required")]
        [Range(0.01, 10000, ErrorMessage = "Quantity must be greater than 0")]
        public decimal Quantity { get; set; }
    }
}
