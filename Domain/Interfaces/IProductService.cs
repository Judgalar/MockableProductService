using DTO;

namespace Domain.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(long id);
    Task<ProductDto> CreateProductAsync(NoIdProductDto product);
    Task<ProductDto> UpdateProductAsync(NoIdProductDto product, long id);
    Task<ProductDto> PatchProductAsync(NoIdProductDto product, long id);
    Task<ProductDto?> DeleteProductAsync(long id);
}
