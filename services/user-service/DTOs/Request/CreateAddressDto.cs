using System.ComponentModel.DataAnnotations;

namespace user_service.DTOs.Request
{
    public class CreateAddressDto
    {
        [Required(ErrorMessage = "Address line is required")]
        [StringLength(255)]
        public string AddressLine { get; set; } = string.Empty;

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? Province { get; set; }

        [StringLength(20)]
        public string? PostalCode { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        public bool IsDefault { get; set; } = false;
    }
}
