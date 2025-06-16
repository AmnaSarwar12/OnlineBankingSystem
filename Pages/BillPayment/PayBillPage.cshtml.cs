using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineBankingSystem.Data;
using OnlineBankingSystem.Model;
using OnlineBankingSystem.Pages.Accounts;

namespace OnlineBankingSystem.Pages.PayBill
{
    public class PayBillPageModel : PageModel
    {
        private readonly BankingDbContext _db;

        public PayBillPageModel(BankingDbContext db)
        {
            _db = db;
        }


        [BindProperty]
        public int FromAccountId {  get; set; }

        [BindProperty]
        public string BillNumber {  get; set; }

        [BindProperty]
        public decimal Amount {  get; set; }

        public List<BankAccount> UserAccounts { get; set; }

        public async Task OnGetAsync()
        {
            UserAccounts = await _db.bankAccounts.ToListAsync();//later:filtered by logged in user
        }

        public async Task<IActionResult> OnPostAsync()
        {
            UserAccounts = await _db.bankAccounts.ToListAsync();//to redisplay dropdown if error 

            if (Amount <= 0)
            {
                ModelState.AddModelError("", "Amount must be greater then zero");
                return Page();
            }
            var fromAccount = await _db.bankAccounts.FindAsync(FromAccountId);
            if(fromAccount == null)
            {
                ModelState.AddModelError("", "Account not found");
                return Page();
            }

            if(fromAccount.Balance < Amount)
            {
                ModelState.AddModelError("", "Insufficient Balance");
                return Page();
            }
            //check minimum balance after payment
            decimal minimumRequiredBalance = 100m;
            if((fromAccount.Balance - Amount)< minimumRequiredBalance)
            {
                ModelState.AddModelError("", $"you must maintain a minimum balance of ${minimumRequiredBalance} after payment.");
                return Page();
            }

            //check for duplicate Bill Number
            bool isDuplicateBill = await _db.payBills.AnyAsync(p=>p.BillNumber == BillNumber);
            if(isDuplicateBill)
            {
                ModelState.AddModelError("", "This Bill is Already been paid");
                return Page();
            }

            //deduct balance
            fromAccount.Balance -= Amount;

            //create billpayment record
            var bill = new  OnlineBankingSystem.Model.PayBill
            {
                BankAccountId = fromAccount.id,
                BillNumber = BillNumber,
                Amount = Amount,
                PaymentDate = DateTime.UtcNow
            };

            _db.payBills.Add(bill);

            //log transaction
            _db.Transaction.Add(new Model.Transactioncs
            {
                BankAccountId = fromAccount.id,
                Amount = -Amount,
                Description = $"Bill Payment: {BillNumber}",
                TimeStamp = DateTime.UtcNow,
                Type = "BillPayment"
            });

            await _db.SaveChangesAsync();
            //for toastr success message
            TempData["BillSuccess"] = true;
            return RedirectToPage("/BillPayment/PayBillPage",new {id =  fromAccount.id});

        }
        
    }
}
