using System.Diagnostics;
using CLDV6212_ST10381071_POEPart1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CLDV6212_ST10381071_POEPart1.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Eventing.Reader;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

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
            if (file != null)
            {
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
            }
            else
            {
                TempData["ErrorMessage"] = "No file selected!";
            }
        
            // redirecting to the index page
            return RedirectToAction("UploadImage");
        }


        // action created - to handle adding a new customer profile
        [HttpPost]
        public async Task<IActionResult> AddCustomerProfile(CustomerProfile profile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // adding the customer profile to the TABLE STORAGE
                    await _tableService.AddEntityAsync(profile);

                    TempData["SuccessMessage"] = "Profile was successfully added!";
                }
                catch(Exception ex)
                {
                    TempData["ErrorMessage"] = "Profile was not added! Plesae try again.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid data!";
            }
                
            // redirecting to the index page
            return RedirectToAction("AddCustomerProfile");
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

        // creating method to call the function - uploadblob
        private async Task<string> CallBlobFunctionAsync(IFormFile file)
        {
            using var content = new MultipartFormDataContent();

            using var stream = file.OpenReadStream();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            content.Add(fileContent, "file", file.FileName);

            var functionUrl = _configuration["FunctionUrls:UploadBlob"]; ;
            functionUrl += $"&containterName=product-images&blobName={file.FileName};"
        
            var response = await _httpClient.PostAsync(functionUrl, content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
