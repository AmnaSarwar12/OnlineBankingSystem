using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using OnlineBankingSystem.Data;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineBankingSystem.Pages.Login
{
    public class LoginPageModel : PageModel
    {

        private readonly BankingDbContext _db;
        private readonly IConfiguration _config;

        public LoginPageModel(BankingDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }


        [BindProperty]
        [Required, EmailAddress]
        public String Email { get; set; }

        [BindProperty]
        [Required]
        public String Password { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user =  _db.Users.FirstOrDefault(u => u.Email == Email && u.Password == Password);
            if (user == null)
            {
                ErrorMessage = "Invalid email and password";
                return Page();
            }

            //generate jwt authentication
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            //store token in a cookie
            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddHours(1),
                SameSite = SameSiteMode.Strict

            });
            HttpContext.Session.SetInt32("UserId", user.Id);

            return RedirectToPage("/DashBoard/DashboardPage");
        }
        
    }
}
