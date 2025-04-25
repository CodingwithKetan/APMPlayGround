using Microsoft.AspNetCore.Mvc;
using OracleManagedAccessWebAPI.Models;
using OracleManagedAccessWebAPI.Services;

namespace OracleManagedAccessWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly OracleService _service;

    public ProductsController(OracleService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
    {
        var products = await _service.GetProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var product = await _service.GetProductByIdAsync(id);
        return product != null ? Ok(product) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        var newId = await _service.CreateProductAsync(product);
        return CreatedAtAction(nameof(Get), new { id = newId }, new { id = newId });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product product)
    {
        if (id != product.Id) return BadRequest();
        await _service.UpdateProductAsync(product);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteProductAsync(id);
        return NoContent();
    }
}