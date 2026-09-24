using Microsoft.EntityFrameworkCore;
using user_service.Data;
using user_service.Models;

namespace user_service.Repositories.Impl
{
    public class ProductReviewRepository : IProductReviewRepository
    {
        private readonly UserDbContext _context;

        public ProductReviewRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductReview>> GetByProductIdAsync(long productId)
        {
            return await _context.ProductReviews
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<ProductReview?> GetByIdAsync(long id)
        {
            return await _context.ProductReviews.FindAsync(id);
        }

        public async Task<ProductReview> CreateAsync(ProductReview review)
        {
            await _context.ProductReviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var review = await _context.ProductReviews.FindAsync(id);
            if (review == null) return false;

            _context.ProductReviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasCustomerReviewedProductAsync(long productId, long customerId)
        {
            return await _context.ProductReviews
                .AnyAsync(r => r.ProductId == productId && r.CustomerId == customerId);
        }
    }
}
