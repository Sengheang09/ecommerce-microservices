using Microsoft.EntityFrameworkCore;
using user_service.Data;
using user_service.Models;

namespace user_service.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly UserDbContext _context;

        public AddressRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Address>> GetByCustomerIdAsync(long customerId)
        {
            return await _context.Addresses
                .Where(a => a.CustomerProfileId == customerId)
                .OrderByDescending(a => a.IsDefault)
                .ThenByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<Address?> GetByIdAsync(long id)
        {
            return await _context.Addresses.FindAsync(id);
        }

        public async Task<Address> CreateAsync(Address address)
        {
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<Address> UpdateAsync(Address address)
        {
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null) return false;

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task ResetDefaultAddressAsync(long customerId)
        {
            var existingDefaults = await _context.Addresses
                .Where(a => a.CustomerProfileId == customerId && a.IsDefault)
                .ToListAsync();

            foreach (var addr in existingDefaults)
            {
                addr.IsDefault = false;
            }

            await _context.SaveChangesAsync();
        }
    }
}
