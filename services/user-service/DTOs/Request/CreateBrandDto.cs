using System.ComponentModel.DataAnnotations;

namespace user_service.DTOs.Request
{
    public class CreateBrandDto
    {
        [Required(ErrorMessage = "Brand name is required")]
        [StringLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
