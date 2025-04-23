using MySqlConnector;
using MySQLWebAPI.Model;

namespace MySQLWebAPI.Services;


public class ProductService : IProductService
{
    private readonly string _connStr;
    public ProductService(IConfiguration config)
    {
        _connStr = config["MySQL:ConnectionString"]!;
        EnsureTable();
    }

    private void EnsureTable()
    {
        using var conn = new MySqlConnection(_connStr);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
          CREATE TABLE IF NOT EXISTS Products (
            Id    INT PRIMARY KEY AUTO_INCREMENT,
            Name  VARCHAR(100) NOT NULL,
            Price DECIMAL(18,2) NOT NULL
          )";
        cmd.ExecuteNonQuery();
    }

    public async Task<int> CreateAsync(Product p)
    {
        await using var conn = new MySqlConnection(_connStr);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
          INSERT INTO Products (Name, Price)
          VALUES (@n, @p);
          SELECT LAST_INSERT_ID();";
        cmd.Parameters.AddWithValue("@n", p.Name);
        cmd.Parameters.AddWithValue("@p", p.Price);
        var raw = await cmd.ExecuteScalarAsync();
        var newId = Convert.ToUInt64(raw);
        return (int)newId;

    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var list = new List<Product>();
        await using var conn = new MySqlConnection(_connStr);
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
        await using var conn = new MySqlConnection(_connStr);
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
        await using var conn = new MySqlConnection(_connStr);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
          UPDATE Products
          SET Name = @n, Price = @p
          WHERE Id = @i";
        cmd.Parameters.AddWithValue("@n", p.Name);
        cmd.Parameters.AddWithValue("@p", p.Price);
        cmd.Parameters.AddWithValue("@i", id);
        var rows = await cmd.ExecuteNonQueryAsync();
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await using var conn = new MySqlConnection(_connStr);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Products WHERE Id = @i";
        cmd.Parameters.AddWithValue("@i", id);
        var rows = await cmd.ExecuteNonQueryAsync();
        return rows > 0;
    }
}
