using Microsoft.EntityFrameworkCore;
using user_service.Data;
using user_service.Models;

namespace user_service.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly UserDbContext _context;

        public CustomerRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerProfile?> GetByIdAsync(long id)
        {
            return await _context.CustomerProfiles
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CustomerProfile?> GetByUserIdAsync(long userId)
        {
            return await _context.CustomerProfiles
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<CustomerProfile> CreateAsync(CustomerProfile customer)
        {
            await _context.CustomerProfiles.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<CustomerProfile> UpdateAsync(CustomerProfile customer)
        {
            customer.UpdatedAt = DateTime.UtcNow;
            _context.CustomerProfiles.Update(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var customer = await _context.CustomerProfiles.FindAsync(id);
            if (customer == null) return false;

            _context.CustomerProfiles.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByUserIdAsync(long userId)
        {
            return await _context.CustomerProfiles.AnyAsync(c => c.UserId == userId);
        }
    }
}