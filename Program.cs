using RedisWebAPI.Services;
using StackExchange.Redis;


var builder = WebApplication.CreateBuilder(args);

// 1) Redis connection multiplexer
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(
        builder.Configuration.GetSection("Redis:ConnectionString").Value!));

// 2) Our cache service
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

// 3) Add controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4) Middleware

    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();