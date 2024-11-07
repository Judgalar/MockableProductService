using System.ComponentModel.DataAnnotations;
using DataAccess.Models;
using DataAccess.Repositories;
using Domain.Interfaces;
using DTO;

namespace Domain.Services;

public class ProductService(IProductRepository repository) : IProductService
{
    private static void Validate(NoIdProductDto productDto)
    {
        ArgumentNullException.ThrowIfNull(productDto);
        ArgumentNullException.ThrowIfNull(productDto.Price);
        ArgumentException.ThrowIfNullOrWhiteSpace(productDto.Name);

        var entityProduct = new Product
        {
            Name = productDto.Name,
            Price = productDto.Price.Value
        };
        
        var validationContext = new ValidationContext(entityProduct);
        var validationResults = new List<ValidationResult>();

        if (Validator.TryValidateObject(entityProduct, validationContext, validationResults, true))
            return;
        
        var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
        throw new ValidationException(errors);
    }
    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        var products = await repository.GetAllAsync();
        return products.Select(p => new ProductDto 
            { 
                Id = p.Id,
                Name = p.Name, 
                Price = p.Price
            }
        ).ToList();
    }

    public async Task<ProductDto?> GetProductByIdAsync(long id)
    {
        ArgumentNullException.ThrowIfNull(id);
        var product = await repository.GetByIdAsync(id);
        return product != null ? new ProductDto 
            { 
                Id = product.Id,
                Name = product.Name, 
                Price = product.Price 
            } 
            : null;
    }

    public async Task<ProductDto> CreateProductAsync(NoIdProductDto productDto)
    {
        Validate(productDto);
        
        var createdProduct = await repository.CreateAsync(new Product { Name = productDto.Name!, Price = productDto.Price!.Value });
        
        return new ProductDto { Id = createdProduct.Id, Name = createdProduct.Name, Price = createdProduct.Price };
    }

    public async Task<ProductDto> UpdateProductAsync(NoIdProductDto productDto, long id)
    {
        Validate(productDto);
        
        var existingProduct = await repository.GetByIdAsync(id);
        
        if (existingProduct == null)
            throw new KeyNotFoundException($"Product with ID {id} was not found.");
        
        existingProduct.Name = productDto.Name!;
        existingProduct.Price = productDto.Price!.Value;
            
        await repository.UpdateAsync(existingProduct);
        
        return new ProductDto {Id = existingProduct.Id, Name = existingProduct.Name, Price = existingProduct.Price};
    }

    public async Task<ProductDto> PatchProductAsync(NoIdProductDto product, long id)
    {
        var existingProduct = await repository.GetByIdAsync(id);
        if (existingProduct == null)
        {
            throw new KeyNotFoundException($"Product with ID {id} was not found.");
        }

        if (!string.IsNullOrWhiteSpace(product.Name))
        {
            existingProduct.Name = product.Name;
            
            // Validación para 'Name' según las reglas definidas en la entidad
            var nameValidationContext = new ValidationContext(existingProduct) { MemberName = nameof(existingProduct.Name) };
            Validator.ValidateProperty(existingProduct.Name, nameValidationContext);
        }

        if (product.Price.HasValue)
        {
            existingProduct.Price = product.Price.Value;
            
            // Validación para 'Price' según las reglas definidas en la entidad
            var priceValidationContext = new ValidationContext(existingProduct) { MemberName = nameof(existingProduct.Price) };
            Validator.ValidateProperty(existingProduct.Price, priceValidationContext);
        }
        
        await repository.UpdateAsync(existingProduct);
        
        return new ProductDto 
        {
            Id = existingProduct.Id, 
            Name = existingProduct.Name, 
            Price = existingProduct.Price
        };
    }

    public async Task<ProductDto?> DeleteProductAsync(long id)
    {
        ArgumentNullException.ThrowIfNull(id);
        var entityProduct = await repository.DeleteAsync(id);
        
        return entityProduct == null 
            ? throw new KeyNotFoundException($"Product with ID {id} was not found.") 
            : new ProductDto
            {
                Id = entityProduct.Id, 
                Name = entityProduct.Name, 
                Price = entityProduct.Price 
            };
    }
}
