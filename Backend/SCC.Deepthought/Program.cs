using System.ClientModel;
using System.Reflection.Metadata.Ecma335;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using Scalar.AspNetCore;
using SCC.Deepthought.Application;
using SCC.Deepthought.Controllers;
using SCC.Deepthought.Domain;
using SCC.Deepthought.Infrastructure;
using Microsoft.Extensions.AI;

var builder = WebApplication.CreateBuilder(args);

// Read the secrets
var azureAiSecrets = new AzureAiSecrets()
{
    AiEndpoint = builder.Configuration["AiEndpoint"] ?? throw new InvalidOperationException("AiEndpoint not available in Secrets."),
    ApiKey = builder.Configuration["ApiKey"] ?? throw new InvalidOperationException("AiEndpoint not available in Secrets."),
    DeploymentName = builder.Configuration["DeploymentName"] ?? throw new InvalidOperationException("AiEndpoint not available in Secrets.")
};

builder.Services.AddSingleton(azureAiSecrets);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

ServiceRegistrations.RegisterServices(builder.Services);

// Register MCP Servers
// Add the MCP Client
var clientTransport = new StdioClientTransport(new StdioClientTransportOptions()
{
    Name = "MCPServer",
    Command = "dotnet",
    Arguments = [
        "run",
        "--project",
        "../MCPServer/MCPServer.csproj"

    ],
});

string endPoint = azureAiSecrets.AiEndpoint;
string key = azureAiSecrets.ApiKey;
var deploymentName = azureAiSecrets.DeploymentName;

var azureClient = new AzureOpenAIClient(new Uri(endPoint), new ApiKeyCredential(key));
var chatClient = azureClient.GetChatClient(deploymentName);
IChatClient client = chatClient.AsIChatClient();

var mcpClient = await McpClient.CreateAsync(clientTransport);
var allTools = await mcpClient.ListToolsAsync();
foreach (var tool in allTools )
{
    Console.WriteLine($"{tool.Name} ({tool.Description})");
}

var chatOptions = new ChatOptions()
{
    ToolMode = ChatToolMode.Auto,
    Tools = [.. allTools]
};

builder.Services.AddSingleton(mcpClient);
builder.Services.AddSingleton(client);
builder.Services.AddSingleton(chatOptions);

var app = builder.Build();

// Ensure the SQLite database and tables are created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SCC.Deepthought.Infrastructure.TicketDbContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

var controllerRegistrations = app.Services.GetService<ControllerRegistrations>();
controllerRegistrations?.RegisterControllers(app);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();


