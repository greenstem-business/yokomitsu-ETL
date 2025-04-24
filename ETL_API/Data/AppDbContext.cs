using ETL_API.Model;
using Microsoft.EntityFrameworkCore;

namespace ETL_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<View_SalesTransaction> SalesTransactions { get; set; }
        public DbSet<Token> Token { get; set; }
        public DbSet<Company> Company { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<View_SalesTransaction>().ToView("VW_Sales_Transactions").HasNoKey();
            modelBuilder.Entity<Token>().ToTable("API_TOKENS");
            modelBuilder.Entity<Company>().ToTable("Company").HasNoKey();
        }
    }
}
