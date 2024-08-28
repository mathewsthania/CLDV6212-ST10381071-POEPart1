using System.Runtime.CompilerServices;
using CLDV6212_ST10381071_POEPart1.Services;
using Microsoft.AspNetCore.Mvc;

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//

namespace CLDV6212_ST10381071_POEPart1.Controllers
{
    public class FileController : Controller
    {
        private readonly FileService _fileService; // creating an instance of the FileService class

        // creating constructor for the file controller
        public FileController(FileService fileService)
        {
            _fileService = fileService;
        }

        // creating action method to display the file upload form
        [HttpGet]
        public IActionResult UploadContract()
        {
            return View();
        }


        // action created to handle a form submission and to upload the file
        [HttpPost]
        public async Task<IActionResult> UploadContract(IFormFile file)
        {
            // checks if the file has been uploaded and not empty
            if(file !=  null && file.Length > 0) 
            {
                using (var stream = file.OpenReadStream())
                {
                    await _fileService.UploadFileAsync("contract-logs", file.FileName, stream);
                }

                // creating message to display if file is uploaded successfully
                ViewBag.Message = "Contract uploaded sucessfully!";
            }
            return View();
        }
    }
}

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*END*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//