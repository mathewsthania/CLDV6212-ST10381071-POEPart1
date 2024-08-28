
//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*START*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//
using Azure.Storage.Files.Shares;

namespace CLDV6212_ST10381071_POEPart1.Services
{
    public class FileService
    {
        // field to hold the ShareServiceClient instance 
        private readonly ShareServiceClient _shareServiceClient;

        // creating constructor to initialize the ShareServiceClient - using the connection string from configuration
        public FileService(IConfiguration configuration)
        {
            _shareServiceClient = new ShareServiceClient(configuration["AzureStorage:ConnectionString"]);
        }

        // method created to upload a file to the specified AZURE FILE SHARE 
        public async Task UploadFileAsync(string shareName, string fileName, Stream content)
        {
            var shareClient = _shareServiceClient.GetShareClient(shareName); // getting a reference to the SHARE 
            await shareClient.CreateIfNotExistsAsync(); // create the SHARE if it doesnt exist already
            var directoryClient = shareClient.GetRootDirectoryClient(); // getting a reference to the root directory of the SHARE
            var fileClient = directoryClient.GetFileClient(fileName); // getting the reference to the file in the directory
            await fileClient.CreateAsync(content.Length); // creating the file with the specified length
            await fileClient.UploadAsync(content); // uploading the file content 
        } 
    }
}

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*END*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//