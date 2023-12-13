namespace ASP_DZ1.Services.Validation
{
    public interface IValidationService
    {
        bool IsNameValid(string name);
        bool IsPhoneValid(string phoneNumber);
        bool IsMailValid(string email);
    }
}
