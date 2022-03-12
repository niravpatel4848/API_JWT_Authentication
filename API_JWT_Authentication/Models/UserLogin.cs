using System.ComponentModel.DataAnnotations;

namespace API_JWT_Authentication.Models
{
    public class UserLogin
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
