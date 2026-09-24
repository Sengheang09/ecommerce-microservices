using user_service.Models;

namespace user_service.Repositories
{
    public interface IProductReviewRepository
    {
        Task<IEnumerable<ProductReview>> GetByProductIdAsync(long productId);
        Task<ProductReview?> GetByIdAsync(long id);
        Task<ProductReview> CreateAsync(ProductReview review);
        Task<bool> DeleteAsync(long id);
        Task<bool> HasCustomerReviewedProductAsync(long productId, long customerId);
    }
}
