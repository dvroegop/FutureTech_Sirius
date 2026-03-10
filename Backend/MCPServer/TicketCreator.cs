using System.ComponentModel;
using ModelContextProtocol.Server;

namespace MCPServer;

[McpServerToolType]
public class TicketCreator
{
    [McpServerTool]
    [Description("Creates a ticket in the backend. Returns the date of creation.")]
    public async Task<string> CreateTicket(string customerName, string description)
    {
        // Here you would have logic to create a ticket in your system.
        // For demonstration purposes, we'll just print the ticket details.
        Console.WriteLine($"Creating ticket for customer: {customerName}");
        Console.WriteLine($"Description: {description}");

        // Simulate some asynchronous work
        await Task.Delay(1000);

        Console.WriteLine("Ticket created successfully.");
        return await Task.FromResult(DateTime.Now.ToLongDateString());
    }

    [McpServerTool]
    [Description("Issues a refund")]
    public async Task<bool> Refund(string customerName, string description, float amount)
    {
        // Here you would have logic to perform a refund
        // For demonstration purposes, we'll just print the details.
        Console.WriteLine($"Creating refund for : {customerName}");
        Console.WriteLine($"Amount: {amount}");

        // Simulate some asynchronous work
        await Task.Delay(1000);

        Console.WriteLine("Refund processed successfully.");
        return true;
    }
}