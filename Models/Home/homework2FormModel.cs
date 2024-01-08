using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
namespace ASP_DZ1.Models.Home
{
    public class homework2FormModel
    {

       
            [FromForm(Name = "user-login")]
            public String Login { get; set; } = null!;

            [FromForm(Name = "user-phone")]
            public String Phone { get; set; } = null!;

            [FromForm(Name = "user-email")]
            public String Email { get; set; } = null!;

            [FromForm(Name = "signup-password")]
            public String Password { get; set; } = null!;

            [FromForm(Name = "signup-repeat")]
            public String Repeat { get; set; } = null!;

            [FromForm(Name = "signup-avatar")]
            [JsonIgnore]
            public IFormFile Avatar { get; set; } = null!;

    }
}
