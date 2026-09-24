using user_service.Enums;

namespace user_service.DTOs.Response
{
    public class CategoryResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public CategoryStatus Status { get; set; }
    }
}
