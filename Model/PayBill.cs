using Microsoft.EntityFrameworkCore;

namespace OnlineBankingSystem.Model
{
    public class PayBill
    {
        public int id {  get; set; }
        public int BankAccountId {  get; set; } //Account paying the bill

        public string BillNumber {  get; set; }

        [Precision(18, 2)]
        public decimal Amount {  get; set; }
        public DateTime PaymentDate {  get; set; }

        public BankAccount BankAccount { get; set; }
    }
}
