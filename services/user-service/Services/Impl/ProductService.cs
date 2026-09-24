using user_service.DTOs.Request;
using user_service.DTOs.Response;
using user_service.Models;
using user_service.Repositories;

namespace user_service.Services.Impl;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<ProductResponseDto>> GetAllAsync(string? search, long? categoryId, long? brandId)
        {
            var products = await _repository.GetAllAsync(search, categoryId, brandId);
            return products.Select(MapToDto);
        }
        public async Task<ProductResponseDto?> GetByIdAsync(long id)
        {
            var product = await _repository.GetByIdAsync(id);
            return product == null ? null : MapToDto(product);
        }
        public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto)
        {
            if (await _repository.ExistsByNameAsync(dto.Name))
            {
                throw new InvalidOperationException($"Product with name '{dto.Name}' already exists.");
            }
            
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Status = dto.Status,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId,
                BrandId = dto.BrandId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            if (dto.Variants != null && dto.Variants.Any())
            {
                product.Variants = dto.Variants.Select(v => new ProductVariant
                {
                    Sku = v.Sku,
                    Price = v.Price,
                    Cost = v.Cost,
                    Weight = v.Weight,
                    Status = v.Status
                    
                }).ToList();
            }
            if (dto.Images != null && dto.Images.Any())
            {
                product.Images = dto.Images.Select(img => new ProductImage
                {
                    ImageUrl = img.ImageUrl,
                    IsPrimary = img.IsPrimary
                }).ToList();
            }
            var created = await _repository.CreateAsync(product);
            var reloaded = await _repository.GetByIdAsync(created.Id);
            return MapToDto(reloaded ?? created);
        }
        public async Task<ProductResponseDto?> UpdateAsync(long id, UpdateProductDto dto)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return null;
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Status = dto.Status;
            product.ImageUrl = dto.ImageUrl;
            product.CategoryId = dto.CategoryId;
            product.BrandId = dto.BrandId;
            product.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(product);
            var reloaded = await _repository.GetByIdAsync(id);
            return MapToDto(reloaded ?? product);
        }
        public async Task<bool> DeleteAsync(long id)
        {
            return await _repository.DeleteAsync(id);
        }
        private static ProductResponseDto MapToDto(Product product)
        {
            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Status = product.Status,
                ImageUrl = product.ImageUrl,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                BrandId = product.BrandId,
                BrandName = product.Brand?.Name,
                Variants = product.Variants.Select(v => new ProductVariantResponseDto
                {
                    Id = v.Id,
                    Sku = v.Sku,
                    Price = v.Price,
                    Cost = v.Cost,
                    Weight = v.Weight,
                    Status = v.Status
                }).ToList(),
                Images = product.Images.Select(img => new ProductImageResponseDto
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl,
                    IsPrimary = img.IsPrimary
                }).ToList()
            };
        }
}