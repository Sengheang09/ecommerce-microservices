using user_service.DTOs.Request;
using user_service.DTOs.Response;
using user_service.Models;
using user_service.Repositories;

namespace user_service.Services.Impl
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly IProductReviewRepository _reviewRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;

        public ProductReviewService(
            IProductReviewRepository reviewRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository)
        {
            _reviewRepository = reviewRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<ProductReviewResponseDto>> GetByProductIdAsync(long productId)
        {
            var reviews = await _reviewRepository.GetByProductIdAsync(productId);
            return reviews.Select(MapToDto);
        }

        public async Task<ProductReviewResponseDto?> GetByIdAsync(long id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            return review == null ? null : MapToDto(review);
        }

        public async Task<ProductReviewResponseDto> CreateAsync(long productId, CreateProductReviewDto dto)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }

            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {dto.CustomerId} not found.");
            }

            var review = new ProductReview
            {
                ProductId = productId,
                CustomerId = dto.CustomerId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                Status = "APPROVED",
                CreatedAt = DateTime.UtcNow
            };

            var created = await _reviewRepository.CreateAsync(review);
            return MapToDto(created);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _reviewRepository.DeleteAsync(id);
        }

        private static ProductReviewResponseDto MapToDto(ProductReview r)
        {
            return new ProductReviewResponseDto
            {
                Id = r.Id,
                ProductId = r.ProductId,
                CustomerId = r.CustomerId,
                Rating = r.Rating,
                Comment = r.Comment,
                Status = r.Status,
                CreatedAt = r.CreatedAt
            };
        }
    }
}
