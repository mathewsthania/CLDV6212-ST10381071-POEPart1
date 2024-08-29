using Azure.Data.Tables;
using CLDV6212_ST10381071_POEPart1;
using CLDV6212_ST10381071_POEPart1.Models;
using System.Threading.Tasks;

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*START*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//

namespace CLDV6212_ST10381071_POEPart1.Services
{
    public class TableService
    {
        // creating field to hold the tableClient instance
        private readonly TableClient _tableClient;

        // creating constructor to initialize the TableClient - with the connection string
        public TableService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureStorage:ConnectionString"]; // retreiving the connection string from the configuration
            var serviceClient = new TableServiceClient(connectionString); // creating a new instance of TableServiceClient using the connection string
            _tableClient = serviceClient.GetTableClient("CustomerProfiles"); // getting a reference to the customerProfiles TABLE
            _tableClient.CreateIfNotExists(); // creating table if it doesnt already exist
        }

        // method to add an entity(customerProfile) to the table
        public async Task AddEntityAsync(CustomerProfile profile)
        {
            // adding the entity (customerProfile) to the table
            await _tableClient.AddEntityAsync(profile);
        }
    }
}

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*END*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//