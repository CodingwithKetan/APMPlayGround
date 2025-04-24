using System;

namespace ElasticSearchWebAPI.Model;

public class Book
{
    public string Id     { get; set; } = Guid.NewGuid().ToString();
    public string Title  { get; set; } = default!;
    public string Author { get; set; } = default!;
    public string Content { get; set; } = default!;
}