using Microsoft.VisualBasic;
using Azure;
using Azure.AI.OpenAI;

namespace BackendAPI.Models
{
    public class UpdateCodeModel
    {
        public string FileName { get; set; }
        public string ModifiedContent { get; set; }
    }
}
