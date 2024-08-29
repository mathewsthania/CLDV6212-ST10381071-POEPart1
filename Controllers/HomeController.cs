using System.Diagnostics;
using CLDV6212_ST10381071_POEPart1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using CLDV6212_ST10381071_POEPart1.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CLDV6212_ST10381071_POEPart1.Controllers
{
    public class HomeController : Controller
    {
        // declaring private readonly fields for the different storage services 
        private readonly BlobService _blobService;
        private readonly TableService _tableService;
        private readonly QueueService _queueService;
        private readonly FileService _fileService;

        // creating constructor to initialize the services - with dependency injection
        public HomeController(BlobService blobService, TableService tableService, QueueService queueService, FileService fileService)
        {
            _blobService = blobService;
            _tableService = tableService;
            _queueService = queueService;
            _fileService = fileService;
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

        public IActionResult SendMessage()
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
                using var stream = file.OpenReadStream();

                // uploading the file to the BLOB STORAGE
                await _blobService.UploadBlobAsync("product-images", file.FileName, stream);
            }

            // redirecting to the index page
            return RedirectToAction("Index");
        }


        // action created - to handle adding a new customer profile
        [HttpPost]
        public async Task<IActionResult> AddCustomerProfile(customerProfile profile)
        {
            if (ModelState.IsValid)
            {
                // adding the customer profile to the TABLE STORAGE
                await _tableService.AddEntityAsync(profile);
            }

            // redirecting to the index page
            return RedirectToAction("Index");
        }


        // action created - to handle processing an order
        [HttpPost]
        public async Task<IActionResult> ProcessOrder(string orderID)
        {
            // sending a message to the queue to process the order 
            await _queueService.SendMessageAsync("order-processing", $"Processing order {orderID}");
            
            // redirecting to the index page 
            return RedirectToAction("Index");
        }

        // action created - to handle uploading a contract
        [HttpPost]
        public async Task<IActionResult> UploadContract(IFormFile file)
        {
            if (file != null)
            {
                using var stream = file.OpenReadStream();
                
                // uploading gile to the FILE SHARE
                await _fileService.UploadFileAsync("contracts-logs", file.FileName, stream);
            }

            // redirecting to the index page
            return RedirectToAction("Index");
        }
    }
}
