using System.ComponentModel.DataAnnotations;

namespace user_service.DTOs.Request
{
    public class CheckoutDto
    {
        [Required(ErrorMessage = "CustomerId is required")]
        public long CustomerId { get; set; }

        [Required(ErrorMessage = "AddressId is required")]
        public long AddressId { get; set; }

        public decimal Discount { get; set; } = 0;
        public decimal ShippingFee { get; set; } = 0;
    }

    public class UpdateOrderStatusDto
    {
        [Required(ErrorMessage = "Status is required")]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;
    }
}
