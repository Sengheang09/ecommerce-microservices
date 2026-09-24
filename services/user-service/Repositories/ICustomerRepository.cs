using user_service.Models;

namespace user_service.Repositories
{
    public interface ICustomerRepository
    {
        Task<CustomerProfile?> GetByIdAsync(long id);
        Task<CustomerProfile?> GetByUserIdAsync(long userId);
        Task<CustomerProfile> CreateAsync(CustomerProfile customer);
        Task<CustomerProfile> UpdateAsync(CustomerProfile customer);
        Task<bool> DeleteAsync(long id);
        Task<bool> ExistsByUserIdAsync(long userId);
    }
}