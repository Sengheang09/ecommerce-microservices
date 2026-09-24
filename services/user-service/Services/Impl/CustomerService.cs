using user_service.DTOs.Request;
using user_service.DTOs.Response;
using user_service.Models;
using user_service.Repositories;

namespace user_service.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<CustomerResponseDto?> GetByIdAsync(long id)
        {
            var customer = await _repository.GetByIdAsync(id);
            return customer == null ? null : MapToDto(customer);
        }

        public async Task<CustomerResponseDto?> GetByUserIdAsync(long userId)
        {
            var customer = await _repository.GetByUserIdAsync(userId);
            return customer == null ? null : MapToDto(customer);
        }

        public async Task<CustomerResponseDto> CreateAsync(CreateCustomerDto dto)
        {
            if (await _repository.ExistsByUserIdAsync(dto.UserId))
            {
                throw new InvalidOperationException($"Customer profile for UserId {dto.UserId} already exists.");
            }

            var customer = new CustomerProfile
            {
                UserId = dto.UserId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Phone = dto.Phone,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var created = await _repository.CreateAsync(customer);
            return MapToDto(created);
        }

        public async Task<CustomerResponseDto?> UpdateAsync(long id, UpdateCustomerDto dto)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null) return null;

            customer.FirstName = dto.FirstName;
            customer.LastName = dto.LastName;
            customer.Phone = dto.Phone;
            customer.DateOfBirth = dto.DateOfBirth;
            customer.Gender = dto.Gender;

            var updated = await _repository.UpdateAsync(customer);
            return MapToDto(updated);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _repository.DeleteAsync(id);
        }

        private static CustomerResponseDto MapToDto(CustomerProfile customer)
        {
            return new CustomerResponseDto
            {
                Id = customer.Id,
                UserId = customer.UserId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.Phone,
                DateOfBirth = customer.DateOfBirth,
                Gender = customer.Gender,
                CreatedAt = customer.CreatedAt,
                UpdatedAt = customer.UpdatedAt
            };
        }
    }
}