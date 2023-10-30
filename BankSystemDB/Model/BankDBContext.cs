using System.Data.Entity;

namespace Homework_18.Model
{
    class BankDBContext : DbContext
    {
        public BankDBContext()
           : base("DBConnectionServer")
        { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Deposit> Deposits { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasRequired<Client>(a => a.Client)
                .WithMany(c => c.Accounts)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Client>()
                .HasRequired<Department>(c => c.Department)
                .WithMany(d => d.Clients)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Deposit>()
                .HasRequired<Client>(d => d.Client)
                .WithMany(c => c.Deposits)
                .WillCascadeOnDelete(true);
        }
    }
}
