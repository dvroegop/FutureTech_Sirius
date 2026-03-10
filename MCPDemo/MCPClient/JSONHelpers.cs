using Json.Schema;
using System.Text.Json;

namespace MCPServer;

internal class JsonHelpers
{
    public string GetSchema()
    {
        var answerSchema = """
                           {
                             "$schema": "https://json-schema.org/draft/2020-12/schema",
                             "title": "QuestionAnswerRecord",
                             "type": "object",
                             "properties": {
                               "OriginalQuestion": {
                                 "type": "string",
                                 "maxLength": 255
                               },
                               "Answer": {
                                 "type": "object",
                                 "properties":{
                                    "State" : {
                                        "type": "string",
                                        "maxLength": 255
                                    },
                                    "Commentary" : {
                                        "type": "string",
                                        "maxLength": 255
                                    }
                                 }
                               }
                             },
                             "required": [
                               "OriginalQuestion",
                               "Answer",
                               "TimeCompleted"
                             ],
                             "additionalProperties": false
                           }
                           """;

        return answerSchema;
    }

    public void ValidateAgainstSchema(string stringContents)
    {
        stringContents = ExtractMarkdownFences(stringContents);
        var answerSchema = GetSchema();

        var schema = JsonSchema.FromText(answerSchema);

        using var jsonDoc = JsonDocument.Parse(stringContents);
        var validationResult = schema.Evaluate(
            jsonDoc.RootElement, 
            new EvaluationOptions
            {
                OutputFormat = OutputFormat.List
            });

        if (validationResult.IsValid)
        {
            Console.WriteLine("JSON validation succeeded!");
        }
        else
        {
            Console.WriteLine("JSON validation failed:");
            foreach (var error in validationResult.Details.Where(d => d.Errors != null))
            {
                foreach (var errorDetail in error.Errors!)
                {
                    Console.WriteLine($"  - {error.InstanceLocation}: {errorDetail.Key} - {errorDetail.Value}");
                }
            }
        }

    }

    private static string ExtractMarkdownFences(string contents)
    {
        // Strip markdown code fences if present
        if (contents.StartsWith("```"))
        {
            var lines = contents.Split('\n');
            contents = string.Join('\n', lines.Skip(1).TakeWhile(l => !l.StartsWith("```")));
        }

        return contents;
    }

    public void InspectError(string toolJson)
    {
        if (!string.IsNullOrWhiteSpace(toolJson))
        {
            try
            {
                using var doc = JsonDocument.Parse(toolJson);
                var root = doc.RootElement;

                var isError = root.TryGetProperty("isError", out var isErrorProp)
                              && isErrorProp.ValueKind == JsonValueKind.True;

                if (isError)
                {
                    string? errorText = null;
                    if (root.TryGetProperty("content", out var contentProp)
                        && contentProp.ValueKind == JsonValueKind.Array
                        && contentProp.GetArrayLength() > 0)
                    {
                        var first = contentProp[0];
                        if (first.ValueKind == JsonValueKind.Object
                            && first.TryGetProperty("text", out var textProp)
                            && textProp.ValueKind == JsonValueKind.String)
                        {
                            errorText = textProp.GetString();
                        }
                    }

                    Console.WriteLine();
                    Console.WriteLine("--- Tool error payload ---");
                    Console.WriteLine(errorText ?? toolJson);
                }
            }
            catch (JsonException)
            {
                // Tool returned a non-JSON payload; just print it.
                Console.WriteLine();
                Console.WriteLine("--- Tool payload (non-JSON) ---");
                Console.WriteLine(toolJson);
            }
        }
    }
}