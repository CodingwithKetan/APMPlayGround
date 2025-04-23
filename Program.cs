using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "PostgreSQL Operations API",
        Version = "v1",
        Description = "A demo API for PostgreSQL database operations with Npgsql"
    });
});

// Print library versions at startup
var npgsqlVersion = typeof(NpgsqlConnection).Assembly.GetName().Version;
Console.WriteLine($"Npgsql Version: {npgsqlVersion}");

var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PostgreSQL Operations API V1");
    });


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();