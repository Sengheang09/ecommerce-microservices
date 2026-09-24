using user_service.DTOs.Request;
using user_service.DTOs.Response;
using user_service.Models;
using user_service.Repositories;

namespace user_service.Services.Impl
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly ICustomerRepository _customerRepository;

        public AddressService(IAddressRepository addressRepository, ICustomerRepository customerRepository)
        {
            _addressRepository = addressRepository;
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<AddressResponseDto>> GetByCustomerIdAsync(long customerId)
        {
            var addresses = await _addressRepository.GetByCustomerIdAsync(customerId);
            return addresses.Select(MapToDto);
        }

        public async Task<AddressResponseDto?> GetByIdAsync(long id)
        {
            var address = await _addressRepository.GetByIdAsync(id);
            return address == null ? null : MapToDto(address);
        }

        public async Task<AddressResponseDto> CreateAsync(long customerId, CreateAddressDto dto)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {customerId} not found.");
            }

            if (dto.IsDefault)
            {
                await _addressRepository.ResetDefaultAddressAsync(customerId);
            }

            var address = new Address
            {
                CustomerProfileId = customerId,
                AddressLine = dto.AddressLine,
                City = dto.City,
                Province = dto.Province,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                IsDefault = dto.IsDefault,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _addressRepository.CreateAsync(address);
            return MapToDto(created);
        }

        public async Task<AddressResponseDto?> UpdateAsync(long id, UpdateAddressDto dto)
        {
            var address = await _addressRepository.GetByIdAsync(id);
            if (address == null) return null;

            if (dto.IsDefault && !address.IsDefault)
            {
                await _addressRepository.ResetDefaultAddressAsync(address.CustomerProfileId);
            }

            address.AddressLine = dto.AddressLine;
            address.City = dto.City;
            address.Province = dto.Province;
            address.PostalCode = dto.PostalCode;
            address.Country = dto.Country;
            address.IsDefault = dto.IsDefault;

            var updated = await _addressRepository.UpdateAsync(address);
            return MapToDto(updated);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _addressRepository.DeleteAsync(id);
        }

        public async Task<bool> SetDefaultAddressAsync(long customerId, long addressId)
        {
            var address = await _addressRepository.GetByIdAsync(addressId);
            if (address == null || address.CustomerProfileId != customerId) return false;

            await _addressRepository.ResetDefaultAddressAsync(customerId);
            address.IsDefault = true;
            await _addressRepository.UpdateAsync(address);

            return true;
        }

        private static AddressResponseDto MapToDto(Address a)
        {
            return new AddressResponseDto
            {
                Id = a.Id,
                CustomerProfileId = a.CustomerProfileId,
                AddressLine = a.AddressLine,
                City = a.City,
                Province = a.Province,
                PostalCode = a.PostalCode,
                Country = a.Country,
                IsDefault = a.IsDefault,
                CreatedAt = a.CreatedAt
            };
        }
    }
}
