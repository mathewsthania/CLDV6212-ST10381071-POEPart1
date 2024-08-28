using CLDV6212_ST10381071_POEPart1.Models;
using CLDV6212_ST10381071_POEPart1.Services;
using Microsoft.AspNetCore.Mvc;

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//

namespace CLDV6212_ST10381071_POEPart1.Controllers
{
    public class TableController : Controller
    {
        
        private readonly TableService _tableService; // creating instance of the table service class

        // creating constructor 
        public TableController(TableService tableService)
        {
            _tableService = tableService;
        }

        // method displays the dorm for adding a new customer profile
        [HttpGet]
        public IActionResult AddCustomerProfile()
        {
            return View();
        }

        // handles the form submission - for adding a new customer profile
        [HttpPost]
        public async Task<IActionResult> AddCustomerProfile(customerProfile profile)
        {
            if (ModelState.IsValid)
            {
                // adding customer profile to the table using table service storage
                await _tableService.AddEntityAsync(profile);
                
                // displaying a success message - if it uploads successfully
                ViewBag.Message = "Customer profile added successfully!";
            }

            // returns the view with the profile data that was inputted - if it was added succesfully or not
            return View(profile);
        }
    }
}

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*END*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//