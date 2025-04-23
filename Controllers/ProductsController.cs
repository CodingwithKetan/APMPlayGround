using Microsoft.AspNetCore.Mvc;
using MSDataSQLClientWebAPI.Model;
using MSDataSQLClientWebAPI.Services;

namespace MSDataSQLClientWebAPI.Controllers;

[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _svc;
    public ProductsController(IProductService svc) => _svc = svc;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _svc.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var p = await _svc.GetByIdAsync(id);
        return p is null ? NotFound() : Ok(p);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product p)
    {
        var id = await _svc.CreateAsync(p);
        p.Id = id;
        return CreatedAtAction(nameof(GetById), new { id }, p);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product p)
    {
        if (!await _svc.UpdateAsync(id, p)) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _svc.DeleteAsync(id)) return NotFound();
        return NoContent();
    }
}