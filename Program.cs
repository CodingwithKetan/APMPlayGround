

using MySQLWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// 1) Register our MySqlService
builder.Services.AddSingleton<IProductService, ProductService>();

// 2) Add controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3) Enable Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();