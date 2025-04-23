using MongoDB.Driver;
using MongoDBWebAPI.Model;
using MongoDBWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// bind settings from configuration
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

// register the TodoService as singleton
builder.Services.AddSingleton<TodoService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ── Print MongoDB.Driver version to console ──
var driverAssembly = typeof(MongoClient).Assembly.GetName();
Console.WriteLine($"MongoDB.Driver version: {driverAssembly.Version}");


// middleware

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();