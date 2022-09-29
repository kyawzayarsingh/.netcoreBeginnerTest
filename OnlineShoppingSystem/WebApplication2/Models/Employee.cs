using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace OnlineShoppingSystem.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User name is required!")]
        public string Name { get; set; }

        [Required,EmailAddress(ErrorMessage = "Enter Valid email Address")]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
