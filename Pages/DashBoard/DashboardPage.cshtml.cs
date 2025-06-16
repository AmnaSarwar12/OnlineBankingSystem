using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineBankingSystem.Data;
using OnlineBankingSystem.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;


namespace OnlineBankingSystem.Pages.DashBoard
{
    [Authorize]
    public class DashboardPageModel : PageModel
    {
        private readonly BankingDbContext _db;

        public DashboardPageModel(BankingDbContext db)
        {
            _db = db;
        }

        public List<Transactioncs> Transactions { get; set; }
        public BankAccount bankAccount { get; set; }
        public string Name { get; set; }

        public decimal totalBalance {  get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Name = User.FindFirst(ClaimTypes.Name)?.Value ?? "User";
            

            var userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null)
            {
                return RedirectToPage("/Login/LoginPage");
            }

            var user = await _db.Users
                .Include(u => u.bankAccounts)
                .ThenInclude(a => a.Transactions)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if(user == null)
            {
                return RedirectToPage("/Login/LoginPage");
            }

            //calculate balance (for one or all accounts)
            totalBalance = user.bankAccounts.Sum(a => a.Balance);

            bankAccount = await _db.bankAccounts.FirstOrDefaultAsync();
            if(bankAccount != null)
            {
                Transactions = await _db.Transaction
                .Where(t => t.BankAccountId == bankAccount.id)
                .OrderByDescending(t => t.TimeStamp)
                .Take(5)
                .ToListAsync();
            }
            return Page();
       
        }
    }
}
