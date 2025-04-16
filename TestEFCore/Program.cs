using Microsoft.EntityFrameworkCore;
using TestEFCore.Data;


var builder = WebApplication.CreateBuilder(args);

// Configure and register the EF Core DbContext with a SQLite connection string.
// In production scenarios, you could move the connection string to appsettings.json.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=myapp.db"));

// Register controller services.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Build the application.
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyApp API V1");
});

// Apply any pending migrations automatically on startup.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Map controller routes.
app.MapControllers();

// Run the application.
app.Run();