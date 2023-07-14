using Azure.Storage.Blobs;

namespace BackendAPI.Services
{
    public class FileOrderService
    {
        public static string? ReadFileOrderLocally(string useCase)
        {
            if (useCase == "SSL" && File.Exists(@"./Data/SSL/FileOrder.txt"))
            {
                return File.ReadAllText(@"./Data/SSL/FileOrder.txt");
            }
            else if (useCase == "AUTH" && File.Exists(@"./Data/AUTH/FileOrder.txt"))
            {
                return File.ReadAllText(@"./Data/AUTH/FileOrder.txt");
            }
            else
            {
                //return Directory.GetCurrentDirectory().ToString();  
                return null;
            }
        }

        public static string GetFileOrderFromBlob(string useCase)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=devexdslbank;AccountKey=SeuyMFcgaAL+kSpbODl+/+ZHWJuZJ0Daufhz7ZGu0ukrazpSUG09/4AIlGKl4E9SdjuodI1RRjuZ+AStLG2D0g==;EndpointSuffix=core.windows.net");

            string containerName = useCase.ToLower();

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient("FileOrder.txt");
            MemoryStream memoryStream = new MemoryStream();
            blobClient.DownloadTo(memoryStream);
            memoryStream.Position = 0;

            string content = new StreamReader(memoryStream).ReadToEnd();
            return content;
        }
    }
}
