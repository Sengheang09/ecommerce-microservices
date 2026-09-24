using user_service.Enums;

namespace user_service.DTOs.Response
{
    public class ProductVariantResponseDto
    {
        public long Id { get; set; }
        public string Sku { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Weight { get; set; }
        public string Status { get; set; } = "ACTIVE";
    }

    public class ProductImageResponseDto
    {
        public long Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
    }

    public class ProductResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ProductStatus Status { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public long CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public long? BrandId { get; set; }
        public string? BrandName { get; set; }

        public List<ProductVariantResponseDto> Variants { get; set; } = new();
        public List<ProductImageResponseDto> Images { get; set; } = new();
    }
}