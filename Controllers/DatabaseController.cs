using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace NPgSQLWebAPI.Controllers;


    [ApiController]
    [Route("[controller]")]
    public class DatabaseController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DatabaseController> _logger;

        public DatabaseController(IConfiguration configuration, ILogger<DatabaseController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("test-query")]
        public async Task<IActionResult> TestQuery()
        {
            await using var conn = new NpgsqlConnection(_configuration.GetConnectionString("Postgres"));
            await conn.OpenAsync();
        
            await using var cmd = new NpgsqlCommand("SELECT version()", conn);
            var result = await cmd.ExecuteScalarAsync();
        
            return Ok(new { 
                DatabaseVersion = result,
                Message = "Check your OpenTelemetry exporter for Npgsql traces"
            });
        }
    }
