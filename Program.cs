using MassTransit;
using MassTransitWebAPI.Consumers;

var builder = WebApplication.CreateBuilder(args);

// 1) Register the consumer
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<TextMessageConsumer>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // Configure endpoint for our consumer
        cfg.ReceiveEndpoint("text-message-queue", e =>
        {
            e.ConfigureConsumer<TextMessageConsumer>(ctx);
        });
    });
});

// 2) Add controllers
builder.Services.AddControllers();
// 3) Swagger (optional)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();