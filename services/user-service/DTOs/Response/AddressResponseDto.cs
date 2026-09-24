namespace user_service.DTOs.Response
{
    public class AddressResponseDto
    {
        public long Id { get; set; }
        public long CustomerProfileId { get; set; }
        public string AddressLine { get; set; } = string.Empty;
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
