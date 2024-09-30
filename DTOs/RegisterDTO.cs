using System.ComponentModel.DataAnnotations;

namespace ServerCozy.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }  // Non-nullable

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } // Non-nullable

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }    // Non-nullable

        public string FirstName { get; set; } // Nullable
        public string LastName { get; set; }  // Nullable
        public string Gender { get; set; }   // Nullable
    }
}
    