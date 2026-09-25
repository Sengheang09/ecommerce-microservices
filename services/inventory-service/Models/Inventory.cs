using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_service.Models
{
    [Table("inventories")]
    public class Inventory
    {
        [Key]
        [Column("product_variant_id")]
        public long ProductVariantId { get; set; }

        [Column("quantity_on_hand", TypeName = "decimal(14,2)")]
        public decimal QuantityOnHand { get; set; } = 0;

        [Column("quantity_reserved", TypeName = "decimal(14,2)")]
        public decimal QuantityReserved { get; set; } = 0;

        [Column("reorder_level", TypeName = "decimal(14,2)")]
        public decimal? ReorderLevel { get; set; } = 0;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public decimal AvailableStock => QuantityOnHand - QuantityReserved;
    }
}