using user_service.DTOs.Request;
using user_service.DTOs.Response;

namespace user_service.Services{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDto>> GetAllAsync(string? search, long? categoryId, long? brandId);
        
        Task<ProductResponseDto?> GetByIdAsync(long id);
        
        Task<ProductResponseDto> CreateAsync(CreateProductDto dto);
        
        Task<ProductResponseDto?> UpdateAsync(long id, UpdateProductDto dto);
        
        Task<bool> DeleteAsync(long id);
        
    }
}