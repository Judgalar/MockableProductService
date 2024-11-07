using DataAccess.Models;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class ProductRepository(MyDbContext dbContext) : IProductRepository
{
    public async Task<List<Product>> GetAllAsync()
    {
        return await dbContext.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(long id)
    {
        return await dbContext.Products.FindAsync(id);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        var entityEntry = await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        var existingProduct = await dbContext.Products.FindAsync(product.Id);
        if (existingProduct == null)
        {
            var addedProduct = await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
            return addedProduct.Entity;
        }

        existingProduct.Name = product.Name;
        existingProduct.Price = product.Price;

        await dbContext.SaveChangesAsync();
        return existingProduct;
    }

    public async Task<Product?> DeleteAsync(long id)
    {
        var existingProduct = await dbContext.Products.FindAsync(id);
        
        if (existingProduct == null)
            return null;
        
        dbContext.Products.Remove(existingProduct);
        await dbContext.SaveChangesAsync();
        
        return existingProduct;
    }
}
