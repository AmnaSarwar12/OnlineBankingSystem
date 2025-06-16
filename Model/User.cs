using System.ComponentModel.DataAnnotations;

namespace OnlineBankingSystem.Model
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required."), EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public ICollection<BankAccount> bankAccounts { get; set; }

    }
}
