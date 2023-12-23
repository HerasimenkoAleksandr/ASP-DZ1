using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ASP_DZ1.Data.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DataSetDateTime RegisterDT { get; set; }

    }
}
