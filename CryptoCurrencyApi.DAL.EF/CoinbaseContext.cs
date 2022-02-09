namespace CryptoCurrencyApi.DAL.EF
{
    using Microsoft.EntityFrameworkCore;

    public partial class CoinbaseContext : DbContext
    {
        public CoinbaseContext(DbContextOptions<CoinbaseContext> options) : base(options)
        {
        }

        public virtual DbSet<Coinbase> Coinbases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coinbase>().ToTable("Coinbase");
        }
    }
}