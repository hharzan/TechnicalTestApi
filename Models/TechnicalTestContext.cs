using Microsoft.EntityFrameworkCore;

namespace TechnicalTestAPI.Models
{
    public class TechnicalTestContext : DbContext
    {
        public TechnicalTestContext(DbContextOptions<TechnicalTestContext> options) : base(options) { }

        public TechnicalTestContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Debtor> Debtors { get; set; }

        public DbSet<Receivable> Receivables { get; set; }
    }
}
