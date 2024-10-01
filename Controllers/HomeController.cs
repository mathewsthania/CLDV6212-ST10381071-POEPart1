using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using CLDV6212_ST10381071_POEPart1.Models;
using CLDV6212_ST10381071_POEPart1.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CLDV6212_ST10381071_POEPart1.Controllers
{
    public class HomeController : Controller
    {
        // declaring private readonly fields for the different storage services 
        private readonly BlobService _blobService;
        private readonly TableService _tableService;
        private readonly QueueService _queueService;
        private readonly FileService _fileService;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        // creating constructor to initialize the services - with dependency injection
        public HomeController(BlobService blobService, TableService tableService, QueueService queueService, FileService fileService, HttpClient httpClient, IConfiguration configuration)
        {
            _blobService = blobService;
            _tableService = tableService;
            _queueService = queueService;
            _fileService = fileService;
            _httpClient = httpClient;
            _configuration = configuration;
            
        }

        // action created - to return the main view
        public IActionResult Index()
        {
            return View();
        }

        // action created - to return the Add Customer Profile view
        public IActionResult AddCustomerProfile()
        {
            return View();
        }

        public IActionResult ProcessOrder()
        {
            return View();
        }

        public IActionResult UploadContract()
        {
            return View();
        }

        public IActionResult UploadImage()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        
        // action created - to return the error view - by requesting ID Information
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        // action created = to handle image uploading 
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "No file selected!";
                return RedirectToAction("UploadImage");
            }
                
            try
            {
                var functionResponse = await CallBlobFunctionAsync(file);

                TempData["SuccessMessage"] = "Image was successfully uploaded!";
            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Image failed to upload,Please try again!";
                Console.WriteLine($"Error: {ex}");
                
            }
            
            // redirecting to the index page
            return RedirectToAction("UploadImage");
        }

        // creating method to call the function - uploadblob
        private async Task<string> CallBlobFunctionAsync(IFormFile file)
        {
            using var content = new MultipartFormDataContent();
            using var stream = file.OpenReadStream();

            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            content.Add(fileContent, "file", file.FileName);

            var functionUrl = _configuration["FunctionUrls:UploadBlob"]; 
            functionUrl += $"&containerName=product-images&blobName={file.FileName}";

            var response = await _httpClient.PostAsync(functionUrl, content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        // action created - to handle adding a new customer profile
        [HttpPost]
        public async Task<IActionResult> AddCustomerProfile(CustomerProfile profile)
        {
            if (profile == null)
            {
                TempData["ErrorMessage"] = "Customer Profile is null, please try again!";
                return RedirectToAction("AddCustomerProfile");
            }

            try
            {
                var functionResponse = await CallStoreTableInfoFunctionAsync(profile);
                TempData["SuccessMessage"] = "Customer Profile was successfully added!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Profile was not added! Please try again.";
                Console.WriteLine($"Error: {ex.Message}");
            }

            // Redirecting to the AddCustomerProfile page
            return RedirectToAction("AddCustomerProfile");
        }

        private async Task<string> CallStoreTableInfoFunctionAsync(CustomerProfile profile)
        {
            // Prepare the request data
            var requestData = new StringContent($"tableName=customerProfiles&partitionKey={profile.PartitionKey}&rowKey={profile.RowKey}&firstName={profile.FirstName}&lastName={profile.LastName}&email={profile.Email}&phoneNumber={profile.PhoneNumber}", Encoding.UTF8, "application/x-www-form-urlencoded");

            var functionUrl = _configuration["FunctionUrls:StoreTableInfo"]; // Adjust based on your configuration
            var response = await _httpClient.PostAsync(functionUrl, requestData);

            response.EnsureSuccessStatusCode(); // Will throw an exception if the response is not a success

            return await response.Content.ReadAsStringAsync();
        }

        // action created - to handle processing an order
        [HttpPost]
        public async Task<IActionResult> ProcessOrder(string orderID)
        {
            if (string.IsNullOrEmpty(orderID))
            {
                TempData["ErrorMessage"] = "Order ID cannot be empty.";
                return RedirectToAction("ProcessOrder");
            }

            try
            {
                var functionUrl = _configuration["FunctionUrls:ProcessQueueMessage"];

                // sending a message to the queue to process the order 
                await _queueService.SendMessageAsync("order-processing", $"Processing order {orderID}");

                TempData["SuccessMessage"] = "Order has been successfully processed!";
            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to process order. Please try again!";
            }
            
            // redirecting to the index page 
            return RedirectToAction("ProcessOrder");
        }


        // action created - to handle uploading a contract
        [HttpPost]
        public async Task<IActionResult> UploadContract(IFormFile file)
        {
            if (file != null)
            {
                try
                {
                    using var stream = file.OpenReadStream();

                    var functionUrl = _configuration["FunctionUrls:UploadFile"];

                    // uploading gile to the FILE SHARE
                    await _fileService.UploadFileAsync("contracts-logs", file.FileName, stream);

                    TempData["SuccessMessage"] = "Contract has been successfully uploaded!";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Failed to upload contract. Please try again!";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "No file selected!";
            }

            // redirecting to the index page
            return RedirectToAction("UploadContract");
        }
    }
}
