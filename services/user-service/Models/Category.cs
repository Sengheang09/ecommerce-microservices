using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using user_service.Enums;

namespace user_service.Models
{
    [Table("categories")]
    public class Category
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Required]
        [Column("status")]
        public CategoryStatus Status { get; set; } = CategoryStatus.Active;

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
