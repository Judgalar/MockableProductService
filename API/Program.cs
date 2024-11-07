using DataAccess;
using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace API;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        
        builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlite(connection));
        
        //Dependency Injection
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IProductService, ProductService>();

        builder.Services.AddControllers();

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
            dbContext.Database.OpenConnection();
            dbContext.Database.EnsureCreated();

            dbContext.Seed();
        }

        app.MapControllers();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.Run();
    }
}
