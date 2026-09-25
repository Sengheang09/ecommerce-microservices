using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_service.Models
{
    [Table("stock_reservations")]
    public class StockReservation
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("product_variant_id")]
        public long ProductVariantId { get; set; }

        [Required]
        [Column("order_id")]
        public long OrderId { get; set; }

        [Required]
        [Column("quantity", TypeName = "decimal(14,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("status")]
        public string Status { get; set; } = "RESERVED"; // RESERVED, CONFIRMED, RELEASED

        [Column("expires_at")]
        public DateTime? ExpiresAt { get; set; }
    }
}