using CLDV6212_ST10381071_POEPart1.Services;
using Microsoft.AspNetCore.Mvc;

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//

namespace CLDV6212_ST10381071_POEPart1.Controllers
{
    public class QueueController : Controller
    {

        private readonly QueueService _queueService; // creating instance of queue service class

        // creating constructor
        public QueueController(QueueService queueService)
        {
            _queueService = queueService;
        }

        // creating action method to display the form for sending a message
        [HttpGet]
        public IActionResult SendMessage()
        {
            return View();
        }

        // creating post method to handle the form submission to send a message to the queue
        [HttpPost]
        public async Task<IActionResult> SendMessage(string message)
        {
            // checking if the message is null or empty
            if (!string.IsNullOrEmpty(message))
            {
                // sends the message to the order-processing queue using queue service storage
                await _queueService.SendMessageAsync("order-processing", message);

                // creating success message if the message is sent succesfully
                ViewBag.Message = "Message sent successfully!";
            }

            return View();
        }
    }
}

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*END*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//
