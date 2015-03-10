using System.ComponentModel.DataAnnotations;

namespace xVal.WebForms.Demo
{
    public class User
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}