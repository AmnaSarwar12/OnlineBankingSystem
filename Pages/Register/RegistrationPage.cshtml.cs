using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBankingSystem.Data;
using OnlineBankingSystem.Model;
using System.ComponentModel.DataAnnotations;

namespace OnlineBankingSystem.Pages.Register
{
    public class RegistrationPageModel : PageModel
    {
        //inserting Db context class inside registration pageModel file
        private readonly BankingDbContext _bankingDbContext;

        public RegistrationPageModel(BankingDbContext bankingDbContext)
        {
            _bankingDbContext = bankingDbContext;
        }

        [BindProperty]
        [Required]
        public String Name { get; set; }

        [BindProperty]
        [Required, EmailAddress]
        public String Email { get; set; }

        [BindProperty]
        [Required, MinLength(6)]
        public String Password { get; set; }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            

            var user = new User
            {
                Name = Name,
                Email = Email,
                Password = Password
            };
            _bankingDbContext.Users.Add(user);

            //this line is to insert the data into the tabel inside the database
            await _bankingDbContext.SaveChangesAsync();

            TempData["RegistrationSuccess"] = "true";

            return RedirectToPage("/Login/LoginPage");
        }
    }
}
