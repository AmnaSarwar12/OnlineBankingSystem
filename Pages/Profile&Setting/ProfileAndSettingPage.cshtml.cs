using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBankingSystem.Data;
using OnlineBankingSystem.Model;
using System.ComponentModel.DataAnnotations;

namespace OnlineBankingSystem.Pages.Profile_Setting
{
    public class ProfileAndSettingPageModel : PageModel
    {
        private readonly BankingDbContext _db;
        private readonly PasswordHasher<User> _passwordHasher;

        public ProfileAndSettingPageModel(BankingDbContext db)
        {
            _db = db;
            _passwordHasher = new PasswordHasher<User>();
        }

        //[BindProperty]
        //public User UserInfo {  get; set; }
        [BindProperty]
        public ProfileUpdate ProfileInput { get; set; }

        //[BindProperty]
        //[DataType(DataType.Password)]
        //public string? NewPassword {  get; set; }
        
        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null)
            {
                return RedirectToPage("/Login/LoginPage");
            }

            var user = await _db.Users.FindAsync(userId);
            if(user == null)
            {
                TempData["Error"] = "User Not Found.";
                return RedirectToPage("/Login/LoginPage");
            }

            ProfileInput = new ProfileUpdate
            {
               // Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null)
            {
                return RedirectToPage("/Login/LoginPage");
            }

            var user = await _db.Users.FindAsync(userId);
            if(user == null)
            {
                TempData["Error"] = "User not found";
                return RedirectToPage("/Login/LoginPage");
            }
            if(!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"{state.Key}: {error.ErrorMessage}");
                    }
                }
                return Page();
            }

            //update basic info
            user.Name = ProfileInput.Name;
            user.Email = ProfileInput.Email;

            //if user entered a new paswword , hash and update
            if (!string.IsNullOrWhiteSpace(ProfileInput.NewPassword))
            {
                user.Password = _passwordHasher.HashPassword(user, ProfileInput.NewPassword);
            }
            try
            {
                await _db.SaveChangesAsync();
                TempData["Success"] = "Profile Updated Successfully";
            }
            catch
            {
                TempData["Error"] = "An error occurred while updating the profile.";
            }
            return RedirectToPage();
        }
        
        
    }
}
