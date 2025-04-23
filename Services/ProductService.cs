using System.Data.SqlClient;
using SystemDataSQlClientWebAPI.Models;

namespace SystemDataSQlClientWebAPI.Services;

public class ProductService : IProductService
{
    private readonly string _connStr;
    public ProductService(IConfiguration config)
    {
        _connStr = config.GetConnectionString("DefaultConnection")!;
        EnsureTable();
    }

    private void EnsureTable()
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
          IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Products')
            CREATE TABLE Products (
              Id    INT IDENTITY(1,1) PRIMARY KEY,
              Name  NVARCHAR(100) NOT NULL,
              Price DECIMAL(18,2)  NOT NULL
            );";
        cmd.ExecuteNonQuery();
    }

    public async Task<int> CreateAsync(Product p)
    {
        await using var conn = new SqlConnection(_connStr);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
          INSERT INTO Products (Name, Price)
          VALUES (@n, @p);
          SELECT CAST(SCOPE_IDENTITY() AS INT);";
        cmd.Parameters.AddWithValue("@n", p.Name);
        cmd.Parameters.AddWithValue("@p", p.Price);
        return (int)await cmd.ExecuteScalarAsync()!;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var list = new List<Product>();
        await using var conn = new SqlConnection(_connStr);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Name, Price FROM Products";
        await using var rdr = await cmd.ExecuteReaderAsync();
        while (await rdr.ReadAsync())
        {
            list.Add(new Product {
              Id    = rdr.GetInt32(0),
              Name  = rdr.GetString(1),
              Price = rdr.GetDecimal(2)
            });
        }
        return list;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        await using var conn = new SqlConnection(_connStr);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Name, Price FROM Products WHERE Id = @i";
        cmd.Parameters.AddWithValue("@i", id);
        await using var rdr = await cmd.ExecuteReaderAsync();
        if (!await rdr.ReadAsync()) return null;
        return new Product {
          Id    = rdr.GetInt32(0),
          Name  = rdr.GetString(1),
          Price = rdr.GetDecimal(2)
        };
    }

    public async Task<bool> UpdateAsync(int id, Product p)
    {
        await using var conn = new SqlConnection(_connStr);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
          UPDATE Products
          SET Name = @n, Price = @p
          WHERE Id = @i";
        cmd.Parameters.AddWithValue("@n", p.Name);
        cmd.Parameters.AddWithValue("@p", p.Price);
        cmd.Parameters.AddWithValue("@i", id);
        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await using var conn = new SqlConnection(_connStr);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Products WHERE Id = @i";
        cmd.Parameters.AddWithValue("@i", id);
        return await cmd.ExecuteNonQueryAsync() > 0;
    }
}