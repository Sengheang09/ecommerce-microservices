namespace user_service.DTOs.Response
{
    public class BrandResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        
    }
}
