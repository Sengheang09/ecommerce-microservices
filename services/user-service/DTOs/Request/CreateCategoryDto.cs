using System.ComponentModel.DataAnnotations;
using user_service.Enums;

namespace user_service.DTOs.Request
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public CategoryStatus Status { get; set; } = CategoryStatus.Active;
    }
}
