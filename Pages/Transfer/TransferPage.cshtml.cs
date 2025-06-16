using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineBankingSystem.Data;
using OnlineBankingSystem.Model;
using System.Security.Claims;

namespace OnlineBankingSystem.Pages.Transfer
{
    public class TransferPageModel : PageModel
    {
        private readonly BankingDbContext _db;

        public TransferPageModel(BankingDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public string ToAccountNumber { get; set; }

        [BindProperty]
        public decimal Amount { get; set; }

        public BankAccount FromAccount { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            FromAccount = await _db.bankAccounts.FirstOrDefaultAsync(b => b.userId == userId);

            if (FromAccount == null)
            {
                TempData["Error"] = "true";
                return RedirectToPage("/Accounts/AccountPage");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Amount <= 0)
            {
                ModelState.AddModelError("", "Invalid input.");
                return Page();
            }

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var fromAccount = await _db.bankAccounts.FirstOrDefaultAsync(b => b.userId == userId);
            var toAccount = await _db.bankAccounts.FirstOrDefaultAsync(a => a.AccountNumber == ToAccountNumber);

            if (fromAccount == null || toAccount == null)
            {
                ModelState.AddModelError("", "One or both accounts not found.");
                return Page();
            }

            if (fromAccount.Balance < Amount)
            {
                ModelState.AddModelError("", "Insufficient balance.");
                return Page();
            }

            // Transfer money
            fromAccount.Balance -= Amount;
            toAccount.Balance += Amount;

            // Log transactions
            _db.Transaction.AddRange(
                new Transactioncs
                {
                    BankAccountId = fromAccount.id,
                    Amount = -Amount,
                    Description = "Transfer sent",
                    TimeStamp = DateTime.UtcNow,
                    Type="transfer"
                },
                new Transactioncs
                {
                    BankAccountId = toAccount.id,
                    Amount = Amount,
                    Description = "Transfer received",
                    TimeStamp = DateTime.UtcNow,
                    Type= "transfer"
                }
            );

            // Log transfer
            _db.Transfers.Add(new Model.Transfer
            {
                fromAccountId = fromAccount.id,
                toAccountId = toAccount.id,
                amount = Amount
            });

            await _db.SaveChangesAsync();
            TempData["Success"] = "true";
            return RedirectToPage("/Accounts/AccountPage", new { id = fromAccount.id });
        }
    }
}
