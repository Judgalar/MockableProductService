using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
}
