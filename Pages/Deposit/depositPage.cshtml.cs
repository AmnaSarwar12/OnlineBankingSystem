using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBankingSystem.Data;
using OnlineBankingSystem.Model;

namespace OnlineBankingSystem.Pages.Deposit
{
    public class depositPageModel : PageModel
    {
        private readonly BankingDbContext _db;

        public depositPageModel(BankingDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public decimal Amount {  get; set; }

        [BindProperty(SupportsGet = true)]
        public int AccountId {  get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            //Console.WriteLine("Deposit form submitted");
            if (Amount <= 0)
            {
                ModelState.AddModelError(String.Empty, "Amount must be greater then zero");
                return Page();
            }

            var account = await _db.bankAccounts.FindAsync(AccountId);
            if (account == null)
            {
                return NotFound();
            }

            //update balance
            account.Balance += Amount;

            //add transaction 
            var transaction = new Transactioncs
            {
                Amount = Amount,
                TimeStamp = DateTime.UtcNow,
                BankAccountId = AccountId,
                Description = "Deposit",
                Type = "Deposit"
            };
            _db.Transaction.Add(transaction);

            //update to database table
            await _db.SaveChangesAsync();

            return RedirectToPage("/Accounts/AccountPage", new { id = AccountId });

        }
        public void OnGet()
        {
        }
    }
}
