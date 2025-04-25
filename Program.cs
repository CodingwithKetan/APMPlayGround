using OracleManagedAccessWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<OracleService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ensure database/table exist on startup
using (var scope = app.Services.CreateScope())
{
    var oracleService = scope.ServiceProvider.GetRequiredService<OracleService>();
    await oracleService.InitializeAsync();
}


app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.Run();