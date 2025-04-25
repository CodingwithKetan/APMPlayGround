using MassTransit;
using MassTransitWebAPI.Contracts;

namespace MassTransitWebAPI.Consumers;

public class TextMessageConsumer : IConsumer<TextMessage>
{
    private readonly ILogger<TextMessageConsumer> _logger;
    public TextMessageConsumer(ILogger<TextMessageConsumer> logger)
        => _logger = logger;

    public Task Consume(ConsumeContext<TextMessage> context)
    {
        _logger.LogInformation(
            "Received at {Time}: {Text}",
            context.Message.Timestamp,
            context.Message.Text);

        // Acknowledge (automatic)
        Console.Write($"Received at {context.Message.Timestamp}: {context.Message.Text}");
        return Task.CompletedTask;
    }
}