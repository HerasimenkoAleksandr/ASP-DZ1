using ASP_DZ1.Data;
using ASP_DZ1.Services.Hash;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP_DZ1.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly DataContext _dataContext;
        private readonly IHashService _hashService;

        public AuthController(DataContext dataContext, IHashService hashService)
        {
            _dataContext = dataContext;
            _hashService = hashService;
        }

        [HttpGet]
        public object Authenticate(String login, String password)
        {
            // шукаємо користувача із заданим логіном
            var user = _dataContext
                .Users
                .FirstOrDefault(u => u.Login == login);
            // якщо не знаходимо - надсилаємо відмову
            if (user == null)
            {
                HttpContext.Response.StatusCode =
                    StatusCodes.Status401Unauthorized;

                return new { status = "Credentials rejected" };
            }
            // користувач знайдений, формуємо DK з паролю, що надіслано, та
            // солі, що зберігається у БД (як при реєстрації)
            String dk = _hashService.HexString(user.PasswordSalt + password);
            if (user.PassworkDk != dk) { 
                HttpContext.Response.StatusCode =
                    StatusCodes.Status401Unauthorized;

                return new { status = "Credentials rejected" };
            }
            // зберігаємо у сесії факт успішної автентифікації
            HttpContext.Session.SetString("AuthUserId", user.Id.ToString());
            return new { status = "OK" };
        }
       
        [HttpDelete]
        public object SignOut()
        {
            // Получаем идентификатор пользователя из сессии
            var userId = HttpContext.Session.GetString("AuthUserId");

            if (userId == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return new { status = "User not authenticated" };
            }

            // Очищаем сессию для пользователя
            HttpContext.Session.Remove("AuthUserId");

            return new { status = "OK" };
        }
    }
}

