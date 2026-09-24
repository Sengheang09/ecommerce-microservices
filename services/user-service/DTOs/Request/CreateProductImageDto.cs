using System.ComponentModel.DataAnnotations;

namespace user_service.DTOs.Request
{
    public class CreateProductImageDto
    {
        [Required(ErrorMessage = "Image URL is required")]
        [StringLength(512)]
        public string ImageUrl { get; set; } = string.Empty;
        
        public bool IsPrimary { get; set; } = false;
    }
}