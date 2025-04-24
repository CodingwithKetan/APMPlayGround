using Microsoft.AspNetCore.Mvc;

namespace HttpClientWebAPI.Controller;

[ApiController]
[Route("api/communication")]
public class CommunicationController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public CommunicationController()
    {
        _httpClient = new HttpClient();
    }

    [HttpGet("call")]
    public async Task<IActionResult> Call([FromQuery] string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return BadRequest("Please provide a valid `url` query parameter.");

        try
        {
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var mediaType = response.Content.Headers.ContentType?.ToString() ?? "text/plain";
            return Content(content, mediaType);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error calling `{url}`: {ex.Message}");
        }
    }
    
    [HttpGet("call-remote")]
    public async Task<IActionResult> CallThirdParty()
    {
        // Example public API
        var url = "https://jsonplaceholder.typicode.com/todos/1";
        var json = await _httpClient.GetStringAsync(url);
        return Content(json, "application/json");
    }
    
    [HttpGet("status")]
    public IActionResult Status()
    {
        return Ok("returning from HttpClient");
    }
}
