
//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*START*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//

using Azure.Storage.Queues;

namespace CLDV6212_ST10381071_POEPart1.Services
{
    public class QueueService
    {
        // creating field to hold the QueueServiceClient instance
        private readonly QueueServiceClient _queueServiceClient;

        // constructor to intialize the QueServiceClient - with connection string 
        public QueueService(IConfiguration configuration)
        {
            _queueServiceClient = new QueueServiceClient(configuration["AzureStorage:ConnectionString"]);
        }

        // method to send a message - to a specific queue 
        public async Task SendMessageAsync(string queueName, string message)
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName); // getting a refernce to the queue 
            await queueClient.CreateIfNotExistsAsync(); // creating the queue - if it doesnt already exist
            await queueClient.SendMessageAsync(message); // sending the message to the queue 
        }
    }
}

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*END*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//