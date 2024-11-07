using System.ComponentModel.DataAnnotations;
using Domain.Interfaces;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("/[controller]")]
public class ProductController(IProductService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] NoIdProductDto product)
    {
        try
        {
            var result = await service.CreateProductAsync(product);
            return Ok(result);
        }
        catch (ArgumentNullException e)
        {
            return BadRequest(e.Message);
        }
        catch (ValidationException e)
        {
            return UnprocessableEntity(e.Message);
        }
        catch (ArgumentException e)
        {
            return BadRequest("Product.Name cannot be null or empty.");
        }
        catch (Exception e)
        {
            return StatusCode(500, "An unexpected error occurred: " + e.Message);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        try
        {
            var products = await service.GetAllProductsAsync();
            return Ok(products);
        }
        catch (Exception e)
        {
            return StatusCode(500, "An unexpected error occurred: " + e.Message);
        }
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetProductById(long id)
    {
        try
        {
            var product = await service.GetProductByIdAsync(id);
            return product != null ? Ok(product) : NotFound();
        }
        catch (ArgumentNullException e)
        {
            return BadRequest("Id cannot be null.");
        }
        catch (Exception e)
        {
            return StatusCode(500, "An unexpected error occurred: " + e.Message);
        }
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateProduct(long id, [FromBody] NoIdProductDto product)
    {
        try
        {
            var p = await service.UpdateProductAsync(product, id);
            return Ok(p);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ArgumentNullException e)
        {
            return BadRequest(e.Message);
        }
        catch (ArgumentException)
        {
            return BadRequest("Name cannot be empty or null.");
        }
        catch (ValidationException e)
        {
            return UnprocessableEntity(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, "An unexpected error occurred: " + e.Message);
        }
    }

    [HttpPatch("{id:long}")]
    public async Task<IActionResult> PatchProduct(long id, [FromBody] NoIdProductDto product)
    {
        try
        {
            var patchedProduct = await service.PatchProductAsync(product, id);
            return Ok(patchedProduct);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ValidationException e)
        {
            return UnprocessableEntity(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, "An unexpected error occurred: " + e.Message);
        }
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteProduct(long id)
    {
        try
        {
            var deleted = await service.DeleteProductAsync(id);
            return Ok(deleted);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, "An unexpected error occurred: " + e.Message);
        }
    }
}
