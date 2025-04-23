using Microsoft.AspNetCore.Mvc;
using MySQLWebAPI.Model;
using MySQLWebAPI.Services;

namespace MySQLWebAPI.Controller;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _svc;
    public ProductsController(IProductService svc) => _svc = svc;

    // GET /products
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _svc.GetAllAsync());

    // GET /products/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var p = await _svc.GetByIdAsync(id);
        return p is null ? NotFound() : Ok(p);
    }

    // POST /products
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product p)
    {
        var newId = await _svc.CreateAsync(p);
        p.Id = newId;
        return CreatedAtAction(nameof(GetById), new { id = newId }, p);
    }

    // PUT /products/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product p)
    {
        if (!await _svc.UpdateAsync(id, p))
            return NotFound();
        return NoContent();
    }

    // DELETE /products/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _svc.DeleteAsync(id))
            return NotFound();
        return NoContent();
    }
}