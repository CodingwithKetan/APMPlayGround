using Microsoft.AspNetCore.Mvc;
using MongoDBWebAPI.Model;
using MongoDBWebAPI.Services;

namespace MongoDBWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly TodoService _service;
    public TodoController(TodoService service) => _service = service;

    [HttpGet("")]
    public async Task<ActionResult<TodoItem>> Get(string id)
    {
        var item = await _service.GetAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TodoItem newItem)
    {
        await _service.CreateAsync(newItem);
        return CreatedAtAction(nameof(Get), new { id = newItem.Id }, newItem);
    }

    [HttpPut("")]
    public async Task<IActionResult> Update(string id, TodoItem updatedItem)
    {
        var existing = await _service.GetAsync(id);
        if (existing is null) return NotFound();
        updatedItem.Id = id;
        await _service.UpdateAsync(id, updatedItem);
        return NoContent();
    }

    [HttpDelete()]
    public async Task<IActionResult> Delete(string id)
    {
        var existing = await _service.GetAsync(id);
        if (existing is null) return NotFound();
        await _service.RemoveAsync(id);
        return NoContent();
    }
}
