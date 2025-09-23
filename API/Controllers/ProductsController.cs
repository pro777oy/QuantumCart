using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ProductsController(IProductRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return Ok(await repository.GetProductsAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repository.GetProductByIdAsync(id);

        if (product == null) return NotFound();

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repository.AddProduct(product);
        if (await repository.SaveChangesAsync())
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
            
        repository.UpdateProduct(product);

        if (await repository.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("Unable to update product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Product>> DeleteProduct(int id)
    {
        var product = await repository.GetProductByIdAsync(id);
        if (product == null) return NotFound();
        
       repository.DeleteProduct(product);
       
       if (await repository.SaveChangesAsync())
       {
           return NoContent();
       }
       return BadRequest("Unable to delete product");
    }

    private bool ProductExists(int id)
    {
        return repository.ProductExists(id);
    }
}