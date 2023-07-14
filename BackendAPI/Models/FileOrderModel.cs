using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Models
{
    public class FileOrderModel
    {
        public string FileName { get; set; }
        public string ModificationOrder { get; set; }
        public string FileDescription { get; set; }
        public string ChangeDescription { get; set; }
    }

}

