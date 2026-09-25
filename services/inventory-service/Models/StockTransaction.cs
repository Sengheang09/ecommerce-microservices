using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_service.Models
{
    [Table("stock_transactions")]
    public class StockTransaction
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("product_variant_id")]
        public long ProductVariantId { get; set; }

        [Required]
        [MaxLength(10)]
        [Column("transaction_type")]
        public string TransactionType { get; set; } = string.Empty; 

        [Required]
        [Column("quantity", TypeName = "decimal(14,2)")]
        public decimal Quantity { get; set; }

        [MaxLength(30)]
        [Column("reference_type")]
        public string? ReferenceType { get; set; }

        [Column("reference_id")]
        public long? ReferenceId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}