using SystemDataSQlClientWebAPI.Models;

namespace SystemDataSQlClientWebAPI.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?>           GetByIdAsync(int id);
    Task<int>                CreateAsync(Product p);
    Task<bool>               UpdateAsync(int id, Product p);
    Task<bool>               DeleteAsync(int id);
}