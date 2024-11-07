using DataAccess.Models;
using DTO;

namespace DataAccess.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(long id);
    Task<Product> CreateAsync(Product product);
    Task<Product> UpdateAsync(Product product);
    Task<Product?> DeleteAsync(long id);
}
