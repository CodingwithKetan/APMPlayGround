using System;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

// 1) Bind Elasticsearch settings
builder.Services.Configure<ElasticsearchConfig>(
    builder.Configuration.GetSection("Elasticsearch"));

// 2) Register a singleton ElasticsearchClient
builder.Services.AddSingleton(sp =>
{
    var cfg = sp.GetRequiredService<IOptions<ElasticsearchConfig>>().Value;
    var settings = new ElasticsearchClientSettings(new Uri(cfg.Uri))
                       .DefaultIndex(cfg.DefaultIndex)
                       .Authentication(new Base64ApiKey(cfg.ApiKeyId, cfg.ApiKey));
    return new ElasticsearchClient(settings);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger in Development

    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

// Config‐binding POCO
public class ElasticsearchConfig
{
    public string Uri { get; set; } = default!;
    public string DefaultIndex { get; set; } = default!;
    public string ApiKeyId { get; set; } = default!;
    public string ApiKey { get; set; } = default!;
}
