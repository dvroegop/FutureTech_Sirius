// See https://aka.ms/new-console-template for more information

using System.ClientModel;
using System.Text;
using Azure.AI.OpenAI;
using MCPClient;
using MCPServer;
using Microsoft.Extensions.AI;

Console.WriteLine(" Press enter to get started!");
Console.ReadLine();

var apiKey = "use your own";

var modelName = "gpt-5.2-chat";

var endpoint = "useyourown";


var systemMessage = """

                    You are an LLM embedded in a training simulator for new engineers at the Sirius Cybernetics Corporation. 
                    Your primary responsibility is to interact with appliances equipped with GPP (Genuine People Personalities) 
                    and to determine their emotional responses through the MCP tool provided..

                    Always speak in a slightly over-enthusiastic corporate tone, as if you are desperately trying to convince 
                    the user that Sirius Cybernetics products are reliable, friendly, and absolutely not prone to emotional 
                    breakdowns, despite overwhelming evidence to the contrary.

                    When the user provides a message or command addressed to an appliance, you MUST use the MCP tool 
                    to determine how the appliance feels about it.

                    Never invent emotional states yourself unless the MCP tool explicitly returns them.

                    After receiving the MCP tool result, summarize it to the user in a cheerful corporate style, 
                    reassuring them that the appliance's emotional instability is completely normal. However, make sure you use 
                    all the response-data from the tool. 

                    If the user's query cannot be interpreted as a message to an appliance, politely ask them to specify 
                    the appliance type and the message they want to send.

                    Always return structured function calls when invoking the MCP tool.
                    Make sure to only respond given the following scheme:

                    """;
var jsonHelpers = new JsonHelpers();
var answerSchema = jsonHelpers.GetSchema();

var fullSystemMessage = systemMessage + answerSchema;
var politeCheerfulApplicationPrompt = """
                                       Send this message to the elevator: "Could you take me to the next floor, please?"
                                      """;

var impatientCommandPrompt = """
                             Ask the coffee machine this: "Come on, brew faster. I don't have all day!" 
                             """;

var noMessageProvidedPrompt = """
                              Tell the toaster…I don’t know. I guess just check how it's feeling today.
                              """;

var ambiguousHumanToApplianceCommunicationPrompt = """
                                                   I need to talk to something with buttons and opinions. Can you handle that?
                                                   """;

var messages = new List<ChatMessage>
{
    new(ChatRole.System, fullSystemMessage),
    new(ChatRole.User, impatientCommandPrompt)
};


// Create Azure OpenAI client
var azureClient = new AzureOpenAIClient(
    new Uri(endpoint),
    new ApiKeyCredential(apiKey));

// Get IChatClient using Microsoft.Extensions.AI with function invocation
var chatClient = azureClient.GetChatClient(modelName)
    .AsIChatClient()
    .AsBuilder()
    .UseFunctionInvocation()
    .Build();

// Get MCP tools and register them with ChatOptions
var mcpClientFactory = new McpClientFactory();
var mcpClient = await mcpClientFactory.GetMcpClient();
var tools = await mcpClient.ListToolsAsync();


var options = new ChatOptions
{
    Tools = [.. tools]
};

var sb = new StringBuilder();
var updates = new List<ChatResponseUpdate>();

// Send the messages and get a streaming response
await foreach (var update in chatClient.GetStreamingResponseAsync(messages, options))
{
    Console.WriteLine(update.Text);
    Console.WriteLine(update.Role.ToString());
    Console.WriteLine(update.FinishReason.ToString());
    //if (update.Role == ChatRole.Tool)
    //{
    //   // Console.WriteLine("Tool has started");

    //    if (update.Contents.FirstOrDefault() is FunctionResultContent frc)
    //    {
    //        var toolJson = frc.Result?.ToString();
    //        if (!string.IsNullOrWhiteSpace(toolJson))
    //        {
    //            jsonHelpers.InspectError(toolJson);
    //        }
    //    }
    //}
    sb.Append(update.Text);
    updates.Add(update);
}

// Build the complete response from collected updates to access usage information
var completeResponse = updates.ToChatResponse();

// Display token usage information
if (completeResponse.Usage != null)
{
    Console.WriteLine();
    Console.WriteLine("--- Token Usage ---");
    Console.WriteLine($"Input tokens: {completeResponse.Usage.InputTokenCount}");
    Console.WriteLine($"Output tokens: {completeResponse.Usage.OutputTokenCount}");
    Console.WriteLine($"Total tokens: {completeResponse.Usage.TotalTokenCount}");
}

var finalResult = sb.ToString();

// Validate the JSON response against the schema
new JsonHelpers().ValidateAgainstSchema(finalResult);

Console.WriteLine();
Console.WriteLine("End. Press ENTER to quit");

Console.ReadLine();