using ASP_DZ1.Models;
using ASP_DZ1.Models.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System;

namespace ASP_DZ1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ViewResult Task_one()
        {
            TransferViewModel model = new TransferViewModel
            {
                DayOfYear = DateTime.Now.DayOfYear
            };

            return View(model);
        }
        public IActionResult Task_two()
        {

            ViewData["later"]= (char)('A' + new Random().Next(26));
          
            return View();
        }
        public IActionResult Task_three()
        {
            return View();
        }
       
        public IActionResult Task_four()
        {
            return View();
        }
        public IActionResult Task_five()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}