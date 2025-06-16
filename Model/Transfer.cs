using Microsoft.EntityFrameworkCore;

namespace OnlineBankingSystem.Model
{
    public class Transfer
    {
        public int Id { get; set; }
        public int fromAccountId {  get; set; }
        public int toAccountId { get; set; }

        [Precision(18,2)]
        public decimal amount { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        public string Description { get; set; } = "Transfer";
        public BankAccount FromAccount {  get; set; }
        public BankAccount ToAccount { get; set; }
    }
}
