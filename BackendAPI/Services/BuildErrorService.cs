using Azure;
using Azure.AI.OpenAI;

namespace BackendAPI.Services
{
    public class BuildErrorService
    {
        //To return Errors and potential fix in JSON format from build output.
        public static string GetFixFromBuild(string buildOutput)
        {
            OpenAIClient client = new(new Uri("https://devex-openai.openai.azure.com/"), new AzureKeyCredential("92ef0980882644e691c11a96959a9514"));
            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                Messages = {
                        new ChatMessage(ChatRole.System, @"You are a smart software developer. When given the output stream data received after an unsuccessful build,
                        recognize the error messages and their potential fixes."),

                        new ChatMessage(ChatRole.User, "The errors of the unsuccessful build is as follows:"),
                        new ChatMessage(ChatRole.User, buildOutput),
                        new ChatMessage(ChatRole.User, @"Return the list of errors encountered for every file along with a short description of their potential fixes and the position of error.
                        Give the result for every file strictly in a structured JSON format with the following keys: ""FilePath"", ""ErrorList"" [ ""LineNumber"", ""ErrorMessage"", ""PotentialFix"" ]
                        Do not leave any key nor add additional keys in any case. Do not change the JSON format.Enclose the response in square brackets []
                        Do not give any note, statement or additional text with JSON in any case"),
                        },
                MaxTokens = 4000,
                Temperature = 0,
            };

            Response<ChatCompletions> response = client.GetChatCompletions(deploymentOrModelName: "GPT35TurboModel", chatCompletionsOptions);

            /* {
               FilePath : "String",
               "ErrorList" : [ 
                   {  LineNumber : "String"
                      ErrorMessage : "String"
                      PotentialFix : "String"
                   },
                   ]
               } */
            var buildErrors = response.Value.Choices[0].Message.Content;

            return buildErrors;
        }

        //To return the modified code after fixing the build errors in the given inputFile
        static public string FixBuildError(string inputFile, string errorList)
        {
            OpenAIClient client = new(new Uri("https://devex-openai.openai.azure.com/"), new AzureKeyCredential("92ef0980882644e691c11a96959a9514"));
            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                Messages = {
                new ChatMessage(ChatRole.System, "You are a smart software developer. When given a file of code and the encountered error details in the file, make the code changes accordingly to fix that error."),
                new ChatMessage(ChatRole.User, "The code file is as follows:"),
                new ChatMessage(ChatRole.User, inputFile),
                new ChatMessage(ChatRole.User, $"The following errors were encountered in the given file: {errorList}"),
                new ChatMessage(ChatRole.User, "Return the modified code to fix given errors."),
                },
                MaxTokens = 4000,
                Temperature = 0,
            };

            Response<ChatCompletions> response = client.GetChatCompletions(deploymentOrModelName: "GPT35TurboModel", chatCompletionsOptions);
            var fixedCode = response.Value.Choices[0].Message.Content;
            return fixedCode;
        }
    }
}
