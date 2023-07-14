using Azure.AI.OpenAI;
using Azure;

namespace BackendAPI.Services
{
    public class AskGPTService
    {
        public static string GetResponseFromGPT(string query)
        {
            OpenAIClient client = new(new Uri("https://devex-openai.openai.azure.com/"), new AzureKeyCredential("92ef0980882644e691c11a96959a9514"));
            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                Messages = {
                new ChatMessage(ChatRole.System, @"You are a helpful AI assistant."),
                new ChatMessage(ChatRole.User, query),
                },
                MaxTokens = 5000,
                Temperature = 0,
            };

            Response<ChatCompletions> response = client.GetChatCompletions(deploymentOrModelName: "GPT35TurboModel", chatCompletionsOptions);
            var genText = response.Value.Choices[0].Message.Content;

            return genText;
        }
    }
}
