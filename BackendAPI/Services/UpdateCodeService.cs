using Azure.AI.OpenAI;
using Azure;
using Azure.Storage.Blobs;

namespace BackendAPI.Services
{
    public static class UpdateCodeService
    {
        public static string? GetFilePrompt(string useCase, string fileName)
        {
            Dictionary<String, String> SSLfilePromts = new Dictionary<string, string>
            {
                { "Utilities.cs", File.ReadAllText(@".\Data\SSL\Utilities PROMPT.txt") },
                { "appsettings.dev.json", File.ReadAllText(@".\Data\SSL\AppDevSettings PROMPT.txt") },
                { "appsettings.json", File.ReadAllText(@".\Data\SSL\AppSettings PROMPT.txt")},
                { "HostBuilder.cs", File.ReadAllText(@".\Data\SSL\HostBuilder PROMPT.txt") },
                { "launchSettings.json", File.ReadAllText(@".\Data\SSL\LaunchSettings PROMPT.txt") }
            };

            Dictionary<String, String> AUTHfilePromts = new Dictionary<string, string>
            {
                { "appsettings.dev.json", File.ReadAllText(@".\Data\AUTH\AppDevSettings PROMPT.txt") },
                { "HostBuilder.cs", File.ReadAllText(@".\Data\AUTH\HostBuilder PROMPT.txt") },
                { "Startup.cs", File.ReadAllText(@".\Data\AUTH\StartUp PROMPT.txt") }
            };

            string? reqPrompt;
            if (useCase == "SSL" && SSLfilePromts.ContainsKey(fileName))
            {
                reqPrompt = SSLfilePromts[fileName];
            }
            else if (useCase == "AUTH" && AUTHfilePromts.ContainsKey(fileName))
            {
                reqPrompt = AUTHfilePromts[fileName];
            }
            else
            {
                reqPrompt = null;
            }

            return reqPrompt;
        }

        public static string? GetFilePromptFromBlob(string useCase, string fileName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=devexdslbank;AccountKey=SeuyMFcgaAL+kSpbODl+/+ZHWJuZJ0Daufhz7ZGu0ukrazpSUG09/4AIlGKl4E9SdjuodI1RRjuZ+AStLG2D0g==;EndpointSuffix=core.windows.net");

            string containerName = useCase.ToLower();

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            Dictionary<String, String> fileNames = new Dictionary<string, string>
            {
                { "Utilities.cs", "Utilities" },
                { "appsettings.dev.json", "AppDevSettings" },
                { "appsettings.json", "AppSettings" },
                { "HostBuilder.cs", "HostBuilder" },
                { "launchSettings.json", "LaunchSettings" },
                { "Startup.cs", "StartUp"}
            };

            BlobClient blobClient = containerClient.GetBlobClient(fileNames[fileName] + " PROMPT.txt");
            MemoryStream memoryStream = new MemoryStream();
            blobClient.DownloadTo(memoryStream);
            memoryStream.Position = 0;

            string content = new StreamReader(memoryStream).ReadToEnd();
            return content;
        }

        public static string GetModifiedCode(string oldCode, string prompt)
        {
            OpenAIClient client = new(new Uri("https://devex-openai.openai.azure.com/"), new AzureKeyCredential("92ef0980882644e691c11a96959a9514"));
            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                Messages = {
                new ChatMessage(ChatRole.System, @"You are a smart software developer. When given the user's file of code and
                its corresponding code modification instructions, make the code changes accordingly and return the final modified code file.
                And don't change any user defined changes or identifier names in this file and make changes tailored to the given code file."),

                new ChatMessage(ChatRole.User, "The new user file is as follows:"),
                new ChatMessage(ChatRole.User, oldCode),

                new ChatMessage(ChatRole.User, prompt),
                new ChatMessage(ChatRole.User, "Return the modified code!"),
                },
                MaxTokens = 4000,
                Temperature = 0,
            };

            Response<ChatCompletions> response = client.GetChatCompletions(deploymentOrModelName: "GPT35TurboModel", chatCompletionsOptions);
            var genText = response.Value.Choices[0].Message.Content;
            return genText;
        }
    }
}
