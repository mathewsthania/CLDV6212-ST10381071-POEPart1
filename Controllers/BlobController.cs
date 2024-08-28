using CLDV6212_ST10381071_POEPart1.Services;
using Microsoft.AspNetCore.Mvc;
//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//

namespace CLDV6212_ST10381071_POEPart1.Controllers
{
    public class BlobController : Controller
    {
        private readonly BlobService _blobService; // creating instance of the blob service class

        // creating constructor 
        public BlobController(BlobService blobService)
        {
            _blobService = blobService;
        }

        // action created to upload a form - to upload an image
        [HttpGet]
        public IActionResult UploadImage()
        {
            return View();
        }

        // action created to handle a form submission and to upload the image file to the blob storage
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            // checking if the selected file is empty
            if (file != null && file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    await _blobService.UploadBlobAsync("product-images", file.FileName, stream);
                }

                // if the file is uploaded - sucess message is displayed
                ViewBag.Message = "File uploaded successflly!";
            }
            return View();
        }
    }
}

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*END*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//