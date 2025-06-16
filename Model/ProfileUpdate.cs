using System.ComponentModel.DataAnnotations;

namespace OnlineBankingSystem.Model
{
    public class ProfileUpdate
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email {  get; set; }

        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string? NewPassword {  get; set; }
    }
}
