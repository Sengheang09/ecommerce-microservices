namespace user_service.DTOs.Response
{
    public class OrderItemResponseDto
    {
        public long Id { get; set; }
        public long ProductVariantId { get; set; }
        public string ProductNameSnapshot { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class OrderStatusHistoryResponseDto
    {
        public long Id { get; set; }
        public string? OldStatus { get; set; }
        public string NewStatus { get; set; } = string.Empty;
        public DateTime ChangedAt { get; set; }
    }

    public class OrderResponseDto
    {
        public long Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public long CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public long AddressId { get; set; }
        public string? ShippingAddressLine { get; set; }
        public string Status { get; set; } = "PENDING";
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<OrderItemResponseDto> Items { get; set; } = new();
        public List<OrderStatusHistoryResponseDto> StatusHistories { get; set; } = new();
    }
}
