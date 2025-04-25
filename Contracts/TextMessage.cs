namespace MassTransitWebAPI.Contracts;

public interface TextMessage
{
    string Text { get; }
    DateTime Timestamp { get; }
}