using System.Collections.Generic;
using System.Threading.Tasks;
using Elastic.Clients.Elasticsearch;
using ElasticSearchWebAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearchWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly ElasticsearchClient _client;

    public DocumentsController(ElasticsearchClient client)
        => _client = client;

    // 1) Index a new Book
    [HttpPost]
    public async Task<ActionResult> Index([FromBody] Book book)
    {
        var response = await _client.IndexAsync(book, i => i
            .Index(book.GetType().Name.ToLower())); // or use default index
        if (response.IsValidResponse)
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        return BadRequest(response);
    }

    // 2) Get by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetById(string id)
    {
        var resp = await _client.GetAsync<Book>(id);
        if (!resp.Found) return NotFound();
        return Ok(resp.Source);
    }

    // 3) Simple full‐text search
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Book>>> Search([FromQuery] string q)
    {
        var resp = await _client.SearchAsync<Book>(s => s
            .Query(qs => qs
                .QueryString(qs2 => qs2.Query(q)))
            .Size(10));
        return Ok(resp.Documents);
    }
}
