using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace user_service.Models
{
    [Table("order_items")]
    public class OrderItem
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("order_id")]
        public long OrderId { get; set; }

        [Column("product_variant_id")]
        public long ProductVariantId { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("product_name_snapshot")]
        public string ProductNameSnapshot { get; set; } = string.Empty;

        [Column("unit_price", TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }

        [Column("quantity", TypeName = "decimal(10,2)")]
        public decimal Quantity { get; set; }

        [Column("subtotal", TypeName = "decimal(14,2)")]
        public decimal Subtotal { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

        [ForeignKey("ProductVariantId")]
        public virtual ProductVariant? ProductVariant { get; set; }
    }
}
