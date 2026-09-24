using user_service.Models;

namespace user_service.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync(string? search, long? categoryId, long? brandId);
        
        Task<Product?> GetByIdAsync(long id);

        Task<Product> CreateAsync(Product product);

        Task<Product> UpdateAsync(Product product);
        
        Task<bool> DeleteAsync(long id);

        Task<bool> ExistsByNameAsync(string name);
    }
}