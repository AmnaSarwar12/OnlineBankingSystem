using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using OnlineBankingSystem.Data;
using OnlineBankingSystem.Model;
using System.Security.Claims;
using System.Security.Principal;

namespace OnlineBankingSystem.Pages.Accounts
{
    
    public class AccountPageModel : PageModel
    {
        private readonly BankingDbContext _db;

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int TotalPages { get; set; }

        public List<Transactioncs> PagedTransaction { get; set; } = new();

        public AccountPageModel(BankingDbContext db)
        {
            _db = db;
        }

        public BankAccount bankAccount { get; set; }
        public async Task<IActionResult> OnGetAsync(int PageNumber = 1)
        {
              this.PageNumber = PageNumber;

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if(userIdClaim == null)
            {
                return RedirectToPage("/DashBoard/DashboardPage");
            }
            int userId = int.Parse(userIdClaim.Value);

            bankAccount = await _db.bankAccounts
                .Include(b => b.Transactions)
                .FirstOrDefaultAsync(b => b.userId == userId);

            if (bankAccount == null)
            {
                return NotFound();
            }

           
            var transaction = bankAccount.Transactions
                .OrderByDescending(t => t.TimeStamp)
                .ToList();

            TotalPages = (int)Math.Ceiling(transaction.Count / (double)PageSize);
            PagedTransaction = transaction
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            return Page();
        }
    }
}
