using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBankingSystem.Data;
using OnlineBankingSystem.Model;

namespace OnlineBankingSystem.Pages.Withdrawl
{
    public class WithdrawlPageModel : PageModel
    {
        private readonly BankingDbContext _db;

        public WithdrawlPageModel(BankingDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public decimal Amount {  get; set; }

        [BindProperty(SupportsGet =true)]
        public int AccountId {  get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if(Amount <= 0)
            {
                ModelState.AddModelError(string.Empty, "Amount must be greater then zero");
                return Page();
            }

            var account = await _db.bankAccounts.FindAsync(AccountId);
            if (account == null)
            {
                return NotFound();
            }
            if(account.Balance < Amount)
            {
                ModelState.AddModelError(string.Empty, "Insufficiant Balance");
                return Page();
            }

            //withdraw
            account.Balance -= Amount;
            var Transaction = new Transactioncs
            {
                Amount = Amount,
                TimeStamp = DateTime.UtcNow,
                BankAccountId = AccountId,
                Description = "Withdrawl",
                Type = "Withdrawl"
            };
            //add to database
            _db.Transaction.Add(Transaction);
            _db.SaveChangesAsync();

            return RedirectToPage("/Accounts/AccountPage", new {id =  AccountId});
        }
        public void OnGet()
        {
        }
    }
}
