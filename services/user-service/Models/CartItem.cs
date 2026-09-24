using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace user_service.Models
{
    [Table("cart_items")]
    public class CartItem
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("cart_id")]
        public long CartId { get; set; }

        [Column("product_variant_id")]
        public long ProductVariantId { get; set; }

        [Column("quantity", TypeName = "decimal(10,2)")]
        public decimal Quantity { get; set; }

        [ForeignKey("CartId")]
        public virtual Cart? Cart { get; set; }

        [ForeignKey("ProductVariantId")]
        public virtual ProductVariant? ProductVariant { get; set; }
    }
}
