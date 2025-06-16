using Microsoft.EntityFrameworkCore;

namespace OnlineBankingSystem.Model
{
    public class Transactioncs
    {
        public int Id { get; set; }// Primary key
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        [Precision(18, 2)]
        public Decimal Amount { get; set; } = 0.0m;
        public String Type {  get; set; } //deposit, withdrwal,transfer
        public String Description {  get; set; }

        // Foreign Key to BankAccount
        public int BankAccountId {  get; set; }
        public BankAccount bankAccount { get; set; }
    }
}
