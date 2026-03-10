using System.Text.Json;
using Microsoft.Extensions.AI;
using SCC.Deepthought.Domain;
using SCC.Deepthought.Infrastructure;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

namespace SCC.Deepthought.AI;

public class TicketValidator(
    ILogger<TicketValidator> logger,
    IChatClient chatClient,
    ChatOptions chatOptions
)
    : ITicketValidator
{
    public async Task<string> ValidateTicket(TicketSummary ticketSummary)
    {
        var ticketSetupTypicalSCC = """
                                    You are a helpful assistant for the Sirius Cybernetics Corporation (SCC), 
                                    that helps with resolving issues from our customers.
                                    Only use information you know is real and coming from SCC. 
                                    Make sure you are aligned with the corporate values of the SCC.
                                    Give a resolution to the ticket based on the information. 
                                    Make sure you use the lingo common in that part of the universe, as this is legally binding.
                                    
                                    Use the MCP tools if available. Make sure to use the responses from the tools in your answer.
                                    Answer the question with a JSON Object, according to the following JSON Schema:
                                    """;

        var jsonHelpers = new JsonHelpers();
        ticketSetupTypicalSCC += jsonHelpers.GetSchema();
        


        var ticketSummaryAsJson = JsonSerializer.Serialize(ticketSummary);

        var messages = new List<ChatMessage>
        {
            new(ChatRole.System, ticketSetupTypicalSCC),
            new(ChatRole.User, ticketSummaryAsJson)
        };
        var responseList = new List<ChatResponseUpdate>();

        // Optionally, use streaming.. if you dare!
        //var result = await chatClient.GetResponseAsync(messages,chatOptions);
        await foreach (var update in chatClient.GetStreamingResponseAsync(messages, chatOptions))
        {
            responseList.Add(update);
            Console.Write(update.Text);
        }

        return "";
        //    return result.Text;
    }

    public async Task<string> ValidateTicketWithToolsAsync(TicketSummary ticketSummary)
    {
        return await Task.FromResult("No idea...");
    }
}