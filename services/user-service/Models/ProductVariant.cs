using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace user_service.Models
{
    [Table("product_variants")]
    public class ProductVariant
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("sku")]
        public string Sku { get; set; } = string.Empty;

        [Required]
        [Column("price", TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Column("cost", TypeName = "decimal(10,2)")]
        public decimal? Cost { get; set; }

        [Column("weight", TypeName = "decimal(10,3)")]
        public decimal? Weight { get; set; }

        [MaxLength(20)]
        [Column("status")]
        public string Status { get; set; } = "ACTIVE";

        [Required]
        [Column("product_id")]
        public long ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}
