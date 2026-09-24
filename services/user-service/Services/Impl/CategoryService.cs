using user_service.DTOs.Request;
using user_service.DTOs.Response;
using user_service.Models;
using user_service.Repositories;

namespace user_service.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();
            return categories.Select(MapToDto);
        }

        public async Task<CategoryResponseDto?> GetByIdAsync(long id)
        {
            var category = await _repository.GetByIdAsync(id);
            return category == null ? null : MapToDto(category);
        }

        public async Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto)
        {
            if (await _repository.ExistsByNameAsync(dto.Name))
            {
                throw new InvalidOperationException($"Category with name '{dto.Name}' already exists.");
            }

            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                Status = dto.Status
            };

            var created = await _repository.CreateAsync(category);
            return MapToDto(created);
        }

        public async Task<CategoryResponseDto?> UpdateAsync(long id, UpdateCategoryDto dto)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null) return null;

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.Status = dto.Status;

            var updated = await _repository.UpdateAsync(category);
            return MapToDto(updated);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _repository.DeleteAsync(id);
        }

        private static CategoryResponseDto MapToDto(Category category)
        {
            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Status = category.Status
            };
        }
    }
}
