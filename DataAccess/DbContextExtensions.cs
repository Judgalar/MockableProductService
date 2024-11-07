using DataAccess.Models;

namespace DataAccess;

public static class DbContextExtensions
{
    public static void Seed(this MyDbContext context)
    {
        if (context.Products.Any()) 
            return;
        
        context.Products.AddRange(
            new Product { Name = "Producto 1", Price = 10.0m },
            new Product { Name = "Producto 2", Price = 15.0m },
            new Product { Name = "Producto 3", Price = 20.0m }
        );

        context.SaveChanges();
    }
}