using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace user_service.Models
{
    [Table("order_status_history")]
    public class OrderStatusHistory
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("order_id")]
        public long OrderId { get; set; }

        [MaxLength(20)]
        [Column("old_status")]
        public string? OldStatus { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("new_status")]
        public string NewStatus { get; set; } = string.Empty;

        [Column("changed_at")]
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }
    }
}
