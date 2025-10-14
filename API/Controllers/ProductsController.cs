using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ProductsController(IGenericRepository<Product> repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        return Ok(await repository.ListAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repository.GetByIdAsync(id);

        if (product == null) return NotFound();

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repository.Add(product);
        if (await repository.SaveAllAsync())
        {
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        return BadRequest("Unable to create product");
    }

    [HttpPut("{id:int}")]

    public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
    {
        if ((product.Id != id) || !ProductExists(id))
        {
            return BadRequest("Product not found");
        }
            
        repository.Update(product);

        if (await repository.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Unable to update product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Product>> DeleteProduct(int id)
    {
        var product = await repository.GetByIdAsync(id);
        if (product == null) return NotFound();
        
       repository.Remove(product);
       
       if (await repository.SaveAllAsync())
       {
           return NoContent();
       }
       return BadRequest("Unable to delete product");
    }
    
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        return Ok();
    }
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        return Ok();
    }

    private bool ProductExists(int id)
    {
        return repository.Exists(id);
    }
}