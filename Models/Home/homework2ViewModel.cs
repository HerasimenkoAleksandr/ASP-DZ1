using Microsoft.AspNetCore.Mvc;
namespace ASP_DZ1.Models.Home
{
    public record homework2ViewModel
    {

        public homework2FormModel ?  FormModel { get; set; }
        public homework3FormValidation? FormValidation { get; set; }
        public bool? FormStatus { get; set; }
    }
}
