using Microsoft.EntityFrameworkCore;

namespace OnlineBankingSystem.Model
{
    public class BankAccount
    {
        public int id {  get; set; }
        public String AccountNumber {  get; set; }

        [Precision(18, 2)]
        public decimal Balance { get; set; }
        public String AccountType {  get; set; }
        public DateTime OpenedOn { get; set; } = DateTime.UtcNow;
        public String Status { get; set; } = "Active";

        // Foreign Key to User
        public int userId {  get; set; }
        public User user { get; set; }

        // One-to-many with Transactions
        public ICollection<Transactioncs> Transactions { get; set; }
    }
}
