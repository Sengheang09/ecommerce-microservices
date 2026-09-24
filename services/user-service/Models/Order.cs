using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace user_service.Models
{
    [Table("orders")]
    public class Order
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("order_number")]
        public string OrderNumber { get; set; } = string.Empty;

        [Column("customer_id")]
        public long CustomerId { get; set; }

        [Column("address_id")]
        public long AddressId { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("status")]
        public string Status { get; set; } = "PENDING";

        [Column("subtotal", TypeName = "decimal(14,2)")]
        public decimal Subtotal { get; set; }

        [Column("discount", TypeName = "decimal(14,2)")]
        public decimal Discount { get; set; } = 0;

        [Column("shipping_fee", TypeName = "decimal(14,2)")]
        public decimal ShippingFee { get; set; } = 0;

        [Column("total", TypeName = "decimal(14,2)")]
        public decimal Total { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("CustomerId")]
        public virtual CustomerProfile? Customer { get; set; }

        [ForeignKey("AddressId")]
        public virtual Address? ShippingAddress { get; set; }

        public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public virtual ICollection<OrderStatusHistory> StatusHistories { get; set; } = new List<OrderStatusHistory>();
    }
}
