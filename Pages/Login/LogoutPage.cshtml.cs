using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineBankingSystem.Pages.Login
{
    public class LogoutPageModel : PageModel
    {
        public IActionResult OnGet()
        {
            //remove jwt Authentication
            Response.Cookies.Delete("jwt");

            //clear session
            HttpContext.Session.Clear();

            //Redirect to Login Page
            return RedirectToPage("/Login/LoginPage");
        }
    }
}
