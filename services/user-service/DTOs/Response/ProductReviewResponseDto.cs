namespace user_service.DTOs.Response
{
    public class ProductReviewResponseDto
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public long CustomerId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public string Status { get; set; } = "PENDING";
        public DateTime CreatedAt { get; set; }
    }
}
