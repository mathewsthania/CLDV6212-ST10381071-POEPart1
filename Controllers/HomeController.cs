using System.Diagnostics;
using CLDV6212_ST10381071_POEPart1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using CLDV6212_ST10381071_POEPart1.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Eventing.Reader;

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
                    using var stream = file.OpenReadStream();

                    // uploading the file to the BLOB STORAGE
                    await _blobService.UploadBlobAsync("product-images", file.FileName, stream);

                    TempData["SuccessMessage"] = "Image was successfully uploaded!";
                }

                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Image failed to upload,Please try again!";
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
    }
}
