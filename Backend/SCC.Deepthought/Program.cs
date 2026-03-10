using System.ClientModel;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using Scalar.AspNetCore;
using SCC.Deepthought.Application;
using SCC.Deepthought.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Read the secrets
var azureAiSecrets = new AzureAiSecrets
{
    AiEndpoint = builder.Configuration["AiEndpoint"] ??
                 throw new InvalidOperationException("AiEndpoint not available in Secrets."),
    ApiKey = builder.Configuration["ApiKey"] ??
             throw new InvalidOperationException("AiEndpoint not available in Secrets."),
    DeploymentName = builder.Configuration["DeploymentName"] ??
                     throw new InvalidOperationException("AiEndpoint not available in Secrets.")
};

builder.Services.AddSingleton(azureAiSecrets);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();

ServiceRegistrations.RegisterServices(builder.Services);

// Register MCP Servers
// Add the MCP Client
var clientTransport = new StdioClientTransport(new StdioClientTransportOptions
{
    Name = "MCPServer",
    Command = "dotnet",
    Arguments =
    [
        "run",
        "--project",
        "../MCPServer/MCPServer.csproj"
    ]
});

var endPoint = azureAiSecrets.AiEndpoint;
var key = azureAiSecrets.ApiKey;
var deploymentName = azureAiSecrets.DeploymentName;

var azureClient = new AzureOpenAIClient(new Uri(endPoint), new ApiKeyCredential(key));
var chatClient = azureClient.GetChatClient(deploymentName)
    .AsIChatClient()
    .AsBuilder()
    .UseFunctionInvocation()
    .Build();

var mcpClient = await McpClient.CreateAsync(clientTransport);
var tools = await mcpClient.ListToolsAsync();

var options = new ChatOptions
{
    Tools = [.. tools]
};
builder.Services.AddSingleton(options);

builder.Services.AddSingleton(chatClient);

var app = builder.Build();



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