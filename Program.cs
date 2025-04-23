using SystemDataSQlClientWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Register the ProductService for CRUD operations
builder.Services.AddSingleton<IProductService, ProductService>();

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger UI in Development

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();