using user_service.Models;

namespace user_service.Repositories
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetByCustomerIdAsync(long customerId);
        Task<Address?> GetByIdAsync(long id);
        Task<Address> CreateAsync(Address address);
        Task<Address> UpdateAsync(Address address);
        Task<bool> DeleteAsync(long id);
        Task ResetDefaultAddressAsync(long customerId);
    }
}
