using user_service.DTOs.Request;
using user_service.DTOs.Response;

namespace user_service.Services
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandResponseDto>> GetAllAsync();
        Task<BrandResponseDto?> GetByIdAsync(long id);
        Task<BrandResponseDto> CreateAsync(CreateBrandDto dto);
        Task<BrandResponseDto?> UpdateAsync(long id, UpdateBrandDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
