using Microsoft.EntityFrameworkCore;
using TestEFCore.Data;

var builder = WebApplication.CreateBuilder(args);

// 1) Configuration: make sure you load your JSON file here
//    (WebApplication.CreateBuilder does this by default)
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile(
        $"appsettings.{builder.Environment.EnvironmentName}.json",
        optional: true, reloadOnChange: true);

// 2) Read your connection string (must match key in appsettings.json)
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(conn))
{
    throw new InvalidOperationException("Missing DefaultConnection string");
}

// 3) Register your DbContext **before** you call Build()
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlite(conn));

// 4) Register controllers, Swagger, etc.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();


app.MapControllers();

app.Run();