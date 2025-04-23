using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDBWebAPI.Model;

namespace MongoDBWebAPI.Services;

public class TodoService
{
    private readonly IMongoCollection<TodoItem> _todoCollection;

    public TodoService(IOptions<MongoDBSettings> settings)
    {
        var client   = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _todoCollection = database.GetCollection<TodoItem>(settings.Value.CollectionName);
    }

    public async Task<List<TodoItem>> GetAsync() =>
        await _todoCollection.Find(_ => true).ToListAsync();

    public async Task<TodoItem?> GetAsync(string id) =>
        await _todoCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(TodoItem newItem) =>
        await _todoCollection.InsertOneAsync(newItem);

    public async Task UpdateAsync(string id, TodoItem updatedItem) =>
        await _todoCollection.ReplaceOneAsync(x => x.Id == id, updatedItem);

    public async Task RemoveAsync(string id) =>
        await _todoCollection.DeleteOneAsync(x => x.Id == id);
}
