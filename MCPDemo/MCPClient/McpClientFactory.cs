using ModelContextProtocol.Client;

namespace MCPClient;

internal class McpClientFactory
{
    private McpClient? _mcpClient;

    public async Task<McpClient> GetMcpClient()
    {
        if (_mcpClient == null)
        {
            // Connect to an already-running MCP server using HTTP transport
            var transport = new HttpClientTransport(new HttpClientTransportOptions
            {
                Endpoint = new Uri("http://localhost:56343/mcp"),
                Name = "MCPServer2"
            });

            // Connect to the MCP server
            _mcpClient = await McpClient.CreateAsync(transport);
        }

        return _mcpClient;
    }
}