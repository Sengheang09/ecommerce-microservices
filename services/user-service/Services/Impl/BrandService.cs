using user_service.DTOs.Request;
using user_service.DTOs.Response;
using user_service.Models;
using user_service.Repositories;

namespace user_service.Services.Impl
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _repository;

        public BrandService(IBrandRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BrandResponseDto>> GetAllAsync()
        {
            var brands = await _repository.GetAllAsync();
            return brands.Select(MapToDto);
        }

        public async Task<BrandResponseDto?> GetByIdAsync(long id)
        {
            var brand = await _repository.GetByIdAsync(id);
            return brand == null ? null : MapToDto(brand);
        }

        public async Task<BrandResponseDto> CreateAsync(CreateBrandDto dto)
        {
            if (await _repository.ExistsByNameAsync(dto.Name))
            {
                throw new InvalidOperationException($"Brand with name '{dto.Name}' already exists.");
            }

            var brand = new Brand
            {
                Name = dto.Name,
                Description = dto.Description
            };

            var created = await _repository.CreateAsync(brand);
            return MapToDto(created);
        }

        public async Task<BrandResponseDto?> UpdateAsync(long id, UpdateBrandDto dto)
        {
            var brand = await _repository.GetByIdAsync(id);
            if (brand == null) return null;

            brand.Name = dto.Name;
            brand.Description = dto.Description;

            var updated = await _repository.UpdateAsync(brand);
            return MapToDto(updated);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _repository.DeleteAsync(id);
        }

        private static BrandResponseDto MapToDto(Brand brand)
        {
            return new BrandResponseDto
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description
            };
        }
    }
}
