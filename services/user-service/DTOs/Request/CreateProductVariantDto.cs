using System.ComponentModel.DataAnnotations;

namespace user_service.DTOs.Request
{
    public class CreateProductVariantDto
    {

        [Required(ErrorMessage = "Sku is required")]
        [StringLength(100)]
        public string Sku{set ; get;} = string.Empty;


        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 1000000.00, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        public decimal? Cost { get; set; }
        public decimal? Weight { get; set; }
        public string Status { get; set; } = "ACTIVE";

    }
}