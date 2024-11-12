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
        private readonly ImageService _imageService;
        private readonly CustomerService _customerService;
        private readonly OrderProcessService _orderProcessService;
        private readonly DocumentService _documentService;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        // creating constructor to initialize the services - with dependency injection
        public HomeController(ImageService imageService, CustomerService customerService, OrderProcessService orderProcessService, DocumentService documentService, HttpClient httpClient, IConfiguration configuration)
        {
            _imageService = imageService;
            _customerService = customerService;
			_orderProcessService = orderProcessService;
            _documentService = documentService;
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
            if (file != null && file.Length > 0)
            {
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        byte[] imageData = memoryStream.ToArray();

                        await _imageService.InsertBlobAsync(imageData);

                        TempData["SuccessMessage"] = "Image was uploaded successfully!";
                        return RedirectToAction("UploadImage");
                    }
                }
                catch
                {
                    TempData["ErrorMessage"] = "Error! An error occurred while uploading the image, please try again.";
                    return RedirectToAction("UploadImage");
                }
            }

            TempData["ErrorMessage"] = "Please select an image to upload!";
            return RedirectToAction("UploadImage");
        }

        // method to upload customerProfile 
        [HttpPost]
        public async Task<IActionResult> AddCustomerProfile(CustomerProfile profile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _customerService.InsertCustomerAsync(profile);
                    TempData["SuccessMessage"] = "Customer profile was added successfully!";
                }

                catch
                {
                    TempData["ErrorMessage"] = "There was an error adding the customer profile, please try again!";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Please fill in all the required fields!";
            }

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

            bool success = await _orderProcessService.ProcessOrderAsync(orderID);

            if (success) 
            {
                TempData["SuccessMessage"] = "Order has been successfully processed!";
				return RedirectToAction("ProcessOrder");
			}

            else
            { 
                TempData["ErrorMessage"] = "Failed to process order. Please try again!";
				return RedirectToAction("ProcessOrder");
			};
        }


        // action created - to handle uploading a contract
        [HttpPost]
        public async Task<IActionResult> UploadContract(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "Error! No document selected for upload.";
                return RedirectToAction("UploadContract");
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    byte[] documentData = memoryStream.ToArray();

                    await _documentService.InsertDocumentAsync(documentData, file.FileName);

                    TempData["SuccessMessage"] = "Document was uploaded successfully!";
                    return RedirectToAction("UploadContract");
                }
            }

            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error! An error occurred while uploading the document, please try again.";
                return RedirectToAction("UploadDocument");
            }
        }
    }
}