using System.ComponentModel.DataAnnotations;
using user_service.Enums;

namespace user_service.DTOs.Request
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ProductStatus Status { get; set; } = ProductStatus.Active;

        [StringLength(100)]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "CategoryId is required")]
        public long CategoryId { get; set; }

        public long? BrandId { get; set; }

         public List<CreateProductVariantDto> Variants { get; set; } = new();
        public List<CreateProductImageDto> Images { get; set; } = new();
    }
}