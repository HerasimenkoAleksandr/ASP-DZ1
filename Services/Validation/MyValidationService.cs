using System.Text.RegularExpressions;

namespace ASP_DZ1.Services.Validation
{
    public class MyValidationService : IValidationService
    {
        public bool IsNameValid(string name)
        {
          
                // Перевірка на наявність символів
                if (string.IsNullOrEmpty(name))
                {
                return false;
                }

                // перевірка на перший символ 
                if (!Char.IsLetter(name[0]))
                {
                    return false;
                }

                // Перевірка що інші симфоли - це букви або цифри
                for (int i = 1; i < name.Length; i++)
                {
                if (!Char.IsLetterOrDigit(name[i]))
                {
                    return false;
                }
                }
                return true;

        }

        public bool IsPhoneValid(string phoneNumber)
        {
            if(phoneNumber==null)
            {
                return false;
            }
            string pattern = @"^\+?\d{12}$|^\+?\d{5}-\d{2}-\d{2}-\d{3}$";
            Regex regex = new Regex(pattern);
            
        if( regex.IsMatch(phoneNumber))
            {
                return true;
            }
            {
                return false;
            }
        }

        public bool IsMailValid(string email)
        {
           if(email==null)
            {
                return false;
            }
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);

          if( regex.IsMatch(email))
            {
                return true;
            }
            else
            {
                return false;

            }
        }
    }
}
