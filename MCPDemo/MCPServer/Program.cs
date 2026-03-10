// See https://aka.ms/new-console-template for more information

var builder = WebApplication.CreateBuilder(args);

#if DEBUG
// Add file logging for debugging (writes to the MCPServer output directory)
builder.Logging.AddConsole();
#endif

builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

// Optional: Add a root endpoint for health checks or diagnostics
app.MapGet("/", () => Results.Ok(new { status = "MCP Server running", endpoint = "/mcp" }));

app.MapMcp("/mcp");

await app.RunAsync();