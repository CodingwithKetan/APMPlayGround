using MassTransit;
using MassTransitWebAPI.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MassTransitWebAPI.Controller;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    readonly IPublishEndpoint _publisher;
    public MessageController(IPublishEndpoint publisher)
        => _publisher = publisher;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] string text)
    {
        await _publisher.Publish<TextMessage>(new
        {
            Text      = text,
            Timestamp = DateTime.UtcNow
        });

        return Accepted(new { Status = "Message published" });
    }
}