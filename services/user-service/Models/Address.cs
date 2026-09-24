using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace user_service.Models
{
    [Table("addresses")]
    public class Address
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("customer_id")]
        public long CustomerProfileId { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("address_line")]
        public string AddressLine { get; set; } = string.Empty;

        [MaxLength(100)]
        [Column("city")]
        public string? City { get; set; }

        [MaxLength(100)]
        [Column("province")]
        public string? Province { get; set; }

        [MaxLength(20)]
        [Column("postal_code")]
        public string? PostalCode { get; set; }

        [MaxLength(100)]
        [Column("country")]
        public string? Country { get; set; }

        [Column("is_default")]
        public bool IsDefault { get; set; } = false;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("CustomerProfileId")]
        public virtual CustomerProfile? CustomerProfile { get; set; }
    }
}