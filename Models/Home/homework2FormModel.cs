﻿using Microsoft.AspNetCore.Mvc;
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

    }
}
