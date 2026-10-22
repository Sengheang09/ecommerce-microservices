namespace inventory_service.Events
{
    public class OrderPlacedItemEvent
    {
        public long ProductVariantId { get; set; }
        public decimal Quantity { get; set; }
    }

    public class OrderPlacedEvent
    {
        public long OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public long CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderPlacedItemEvent> Items { get; set; } = new();
    }
}