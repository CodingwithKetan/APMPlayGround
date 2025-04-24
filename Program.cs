
var builder = WebApplication.CreateBuilder(args);

// 1. Register controllers (if you have any)
builder.Services.AddControllers();

// 2. Register Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3. Enable middleware
app.UseSwagger();                  // <— Requires AddSwaggerGen()
// You can customize the Swagger UI route if you like:
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();