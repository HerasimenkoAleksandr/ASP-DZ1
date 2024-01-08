using ASP_DZ1.Models;
using ASP_DZ1.Models.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System;
using ASP_DZ1.Services.Validation;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using ASP_DZ1.Data;
using ASP_DZ1.Services.Hash;
using ASP_DZ1.Data.Entities;

namespace ASP_DZ1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IValidationService _validation;

        private readonly IHashService _hashService;

        private readonly DataContext _dataContext;

        public HomeController(ILogger<HomeController> logger, IValidationService validation, IHashService hashService, DataContext dataContext)
        {
            _logger = logger;
            _validation = validation;
            _hashService = hashService;
            _dataContext = dataContext;
        }

        public ViewResult homework3()
        {
            homework2ViewModel viewModel = new();

            // перевіряємо, чи є дані від форми
            if (HttpContext.Session.Keys.Contains("formStatus"))
            {
                // декодуємо статус
                viewModel.FormStatus = Convert.ToBoolean(
                    HttpContext.Session.GetString("formStatus"));
                HttpContext.Session.Remove("formStatus");

                // перевіряємо - якщо помилковий, то у сесії дані валідації і моделі
                if (viewModel.FormStatus ?? false)
                {
                    viewModel.FormModel = null;
                    viewModel.FormValidation = null;
                }
                else
                {
                    viewModel.FormModel = JsonSerializer
                        .Deserialize<homework2FormModel>(
                            HttpContext.Session.GetString("formModel")!);
                    HttpContext.Session.Remove("formModel");
                    viewModel.FormValidation = JsonSerializer
                        .Deserialize<homework3FormValidation>(
                            HttpContext.Session.GetString("formValidation")!);
                    HttpContext.Session.Remove("formValidation");
                }
            }

            return View(viewModel);
        }
        [HttpPost]
        public RedirectToActionResult SignupForm(homework2FormModel model)
        {
            homework3FormValidation results = new();
            bool isFormValid = true;
            if (String.IsNullOrEmpty(model.Login))
            {
                results.LoginErrorMessage = "Логін не може бути порожним";

                _validation.IsNameValid(model.Login);
                isFormValid = false;
            }
            else
            {
                if(!_validation.IsNameValid(model.Login))
                {
                    results.LoginErrorMessage = "Формат логіна не вірний";
                    isFormValid = false;
                }
            }
            if (String.IsNullOrEmpty(model.Phone))
            {
                results.PhoneErrorMessage = "Номер телефону не може бути порожним";
                isFormValid = false;
            }
            else
            {
                if (!_validation.IsPhoneValid(model.Phone))
                {
                    results.PhoneErrorMessage = "Формат телефону не вірний";
                    isFormValid = false;
                }
            }
            if (String.IsNullOrEmpty(model.Email))
            {
                results.EmailErrorMessage = "Email не може бути порожним";
                isFormValid = false;
            }
            else
            {
                if (!_validation.IsMailValid(model.Email))
                {
                    results.EmailErrorMessage = "Формат Email не вірний";
                    isFormValid = false;
                }
            }
            if (String.IsNullOrEmpty(model.Password))
            {
                results.PasswordErrorMessage = "Пароль не може бути порожним";
                isFormValid = false;
            }
            if (model.Password != model.Repeat)
            {
                results.RepeatErrorMessage = "Повтор не збігається з паролем";
                isFormValid = false;
            }

            if (isFormValid && model.Avatar != null &&
                model.Avatar.Length > 0)  // поле не обов'язкове, але якщо є, то перевіряємо
            {
                // при збереженні (uploading) файлів слід міняти їх імена.
                int dotPosition = model.Avatar.FileName.LastIndexOf(".");
                if (dotPosition == -1)
                {
                    results.AvatarErrorMessage = "Файли без розширення не приймаються";
                    isFormValid = false;
                }
                else
                {
                    String ext = model.Avatar.FileName.Substring(dotPosition);
                    // TODO: додати перевірку розширення на перелік дозволених

                    // генеруємо випадкове ім'я файлу, зберігаємо розширення
                    // контролюємо, що такого імені немає у сховищі
                    String dir = Directory.GetCurrentDirectory();
                    String savedName;
                    String fileName;
                    do
                    {
                        fileName = Guid.NewGuid() + ext;
                        savedName = Path.Combine(dir, "wwwroot", "avatars", fileName);
                    }
                    while (System.IO.File.Exists(savedName));
                    using Stream stream = System.IO.File.OpenWrite(savedName);
                    model.Avatar.CopyTo(stream);

                    // до цього місця доходимо у разі відсутності помилок валідації
                    // додаємо нового користувача до БД
                    String salt = _hashService.HexString(Guid.NewGuid().ToString());
                    String dk = _hashService.HexString(salt + model.Password);
                    _dataContext.Users.Add(new()
                    {
                        Id = Guid.NewGuid(),
                        Login = model.Login,
                        Phone = model.Phone,
                        Avatar = fileName,
                        RegisterDt = DateTime.Now,
                        DeleteDt = null,
                        Email = model.Email,
                        PasswordSalt = salt,
                        PassworkDk = dk,
                    });
                    _dataContext.SaveChanges();
                }
            }
            if (!isFormValid)
            {
                HttpContext.Session.SetString("formModel",
                    JsonSerializer.Serialize(model));

                HttpContext.Session.SetString("formValidation",
                    JsonSerializer.Serialize(results));
            }
            HttpContext.Session.SetString("formStatus",
                isFormValid.ToString());

            return RedirectToAction(nameof(homework3));
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