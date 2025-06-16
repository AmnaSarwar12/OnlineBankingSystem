using Microsoft.EntityFrameworkCore;
using OnlineBankingSystem.Model;
using System.Reflection.Emit;

namespace OnlineBankingSystem.Data
{
    public class BankingDbContext : DbContext
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options) { }

        //this represents the users table in my database
        public DbSet<User> Users { get; set; }
        public DbSet<BankAccount> bankAccounts { get; set; }
        public DbSet<Transactioncs> Transaction { get; set; }

        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<PayBill> payBills { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            

        modelBuilder.Entity<BankAccount>()
                .HasOne(b => b.user)
                .WithMany(u => u.bankAccounts)
                .HasForeignKey(b => b.userId);

        modelBuilder.Entity<Transactioncs>()
                .HasOne(t => t.bankAccount)
                .WithMany(b => b.Transactions)
                .HasForeignKey(t => t.BankAccountId);

            modelBuilder.Entity<Transfer>()
                    .HasOne(t => t.FromAccount)
                    .WithMany()
                    .HasForeignKey(t => t.fromAccountId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transfer>()
                .HasOne(t=> t.ToAccount)
                .WithMany()
                .HasForeignKey(t=>t.toAccountId)
                .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<BankAccount>().HasData(
                new BankAccount
                {
                    id = 1,
                    AccountNumber = "123456789",
                    Balance = 5000.00m,
                    AccountType = "Savings",
                    OpenedOn = new DateTime(2024, 12, 01),
                    Status = "Active",
                    userId = 1
                }
                );

            modelBuilder.Entity<Transactioncs>().HasData(
                new Transactioncs
                {
                    Id = 1,
                    TimeStamp = new DateTime(2024, 12, 01),
                    Amount = 150.0m,
                    Type = "Deposit",
                    Description = "my first transaction",
                    BankAccountId = 1

                },

                new Transactioncs
                {
                    Id = 2,
                    TimeStamp = new DateTime(2024, 12, 01),
                    Amount = 150.0m,
                    Type = "Withdraw",
                    Description = "my Second transaction",
                    BankAccountId = 1

                }
                );
    }
}
}
