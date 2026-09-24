using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace user_service.Models
{
    [Table("product_images")]
    public class ProductImage
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [MaxLength(512)]
        [Column("image_url")]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [Column("is_primary")]
        public bool IsPrimary { get; set; } = false;

        [Required]
        [Column("product_id")]
        public long ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}
