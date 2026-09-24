namespace user_service.DTOs.Response
{
    public class CartItemResponseDto
    {
        public long Id { get; set; }
        public long ProductVariantId { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal Subtotal => UnitPrice * Quantity;
    }

    public class CartResponseDto
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<CartItemResponseDto> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(i => i.Subtotal);
    }
}
