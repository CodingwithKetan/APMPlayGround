using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBWebAPI.Model;

public class TodoItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id          { get; set; }

    [BsonElement("name")]
    public string   Name       { get; set; } = null!;

    [BsonElement("isComplete")]
    public bool     IsComplete { get; set; }
}