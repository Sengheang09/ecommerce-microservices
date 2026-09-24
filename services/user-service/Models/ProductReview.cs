using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace user_service.Models
{
    [Table("product_reviews")]
    public class ProductReview
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("rating")]
        public int Rating { get; set; }

        [Column("comment")]
        public string? Comment { get; set; }

        [MaxLength(20)]
        [Column("status")]
        public string Status { get; set; } = "PENDING";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("product_id")]
        public long ProductId { get; set; }

        [Required]
        [Column("customer_id")]
        public long CustomerId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}
