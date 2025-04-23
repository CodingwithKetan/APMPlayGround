using MSDataSQLClientWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// 1) Configure connection string
var connStr = builder.Configuration.GetConnectionString("DefaultConnection");

// 2) Register the ProductService for CRUD
builder.Services.AddSingleton<IProductService, ProductService>();

// 3) Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();