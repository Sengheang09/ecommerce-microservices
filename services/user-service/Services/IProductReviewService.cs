using user_service.DTOs.Request;
using user_service.DTOs.Response;

namespace user_service.Services
{
    public interface IProductReviewService
    {
        Task<IEnumerable<ProductReviewResponseDto>> GetByProductIdAsync(long productId);
        Task<ProductReviewResponseDto?> GetByIdAsync(long id);
        Task<ProductReviewResponseDto> CreateAsync(long productId, CreateProductReviewDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
