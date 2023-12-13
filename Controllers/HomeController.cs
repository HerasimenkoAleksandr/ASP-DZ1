using ASP_DZ1.Models;
using ASP_DZ1.Models.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System;
using ASP_DZ1.Services.Validation;
using System.Security.Cryptography.Xml;
using System.Text.Json;

namespace ASP_DZ1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IValidationService _validation;

        public HomeController(ILogger<HomeController> logger, IValidationService validation)
        {
            _logger = logger;
            _validation = validation;
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

        public ViewResult homework2()
        {
            homework2FormModel ? formModel;

            if (HttpContext.Session.Keys.Contains("formModel"))
            {
                // Є збережені у сесії дані - відновлюємо їх та видаляємо з сесії
                formModel = JsonSerializer.Deserialize<homework2FormModel>(
                    HttpContext.Session.GetString("formModel")!
                );
                HttpContext.Session.Remove("formModel");
            }
            else
            {
                formModel = null!;
            }
          
            homework2ViewModel model= new()
            {
                FormModel = formModel
            };

            
            if (model.FormModel != null || model.FormModel?.Login != null)
            {

                ViewData["isNameV"] = _validation.IsNameValid(model.FormModel.Login);
            }



            if (model.FormModel != null || model.FormModel?.Phone != null)
            {
                ViewData["isNameP"] = _validation.IsPhoneValid(model.FormModel.Phone);
            }


            if (model.FormModel != null ||  model.FormModel?.Email != null)
            {
          
                ViewData["isNameE"] = _validation.IsMailValid(model.FormModel.Email);
            }


            return View(model);
        }

        [HttpPost]
        public IActionResult ProcessHomework2(homework2FormModel? formModel)
        {
            if (formModel != null)
            {
                // зберігаємо у сесії серіалізований об'єкт formModel під
                // іменем "formModel"
                HttpContext.Session.SetString(
                    "formModel",
                    JsonSerializer.Serialize(formModel)
                );
            }
            return RedirectToAction(nameof(homework2));
          
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}