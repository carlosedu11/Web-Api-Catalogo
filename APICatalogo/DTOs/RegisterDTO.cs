using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace APICatalogo.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Username is required!")]
        public string? Username{ get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required!")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        public string? Password { get; set; }
    }
}
