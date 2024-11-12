using System.ComponentModel.DataAnnotations;
using DataAccess.Models;
using DataAccess.Repositories;
using Domain.Services;
using DTO;
using Moq;

namespace Test;

public class ProductServiceTests
{
    private ProductService _service;
    private Mock<IProductRepository> _mockRepository;
    
    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IProductRepository>();
        
        _mockRepository
            .Setup(repo => repo.CreateAsync(It.IsAny<Product>()))
            .ReturnsAsync((Product p) => new Product { Name = p.Name, Price = p.Price });
        
        _service = new ProductService(_mockRepository.Object);
    }

    [Test]
    public async Task CreateProductAsync_ShouldReturnCreatedProduct()
    {
        var inputDto = new NoIdProductDto() {Name = "test", Price = 10};
        var outputDto = await _service.CreateProductAsync(inputDto);
        
        Assert.Multiple(() =>
        {
            Assert.That(outputDto.Name, Is.EqualTo(inputDto.Name));
            Assert.That(outputDto.Price, Is.EqualTo(inputDto.Price));
        });
        
        _mockRepository.Verify(repo => repo.CreateAsync(It.Is<Product>(p => 
            p.Name == inputDto.Name && p.Price == inputDto.Price)), Times.Once);
    }

    [Test]
    public void CreateProductAsync_ShouldNotReturnCreatedProduct()
    {
        var inputDto = new NoIdProductDto() {Name = "test", Price = 0};
        
        var ex = Assert.ThrowsAsync<ValidationException>(async () => 
            await _service.CreateProductAsync(inputDto));
        
        Assert.That(ex.Message, Is.EqualTo("The price must be greater than zero"));
    }
}