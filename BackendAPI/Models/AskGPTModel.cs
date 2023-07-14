using Azure.AI.OpenAI;
using Azure;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Models
{
    public class AskGPTModel
    {
        public string queryType { get; set; }
        public string query { get; set; }
    }
    
}
