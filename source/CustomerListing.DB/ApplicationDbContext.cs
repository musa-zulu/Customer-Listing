using CustomerListing.DB.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CustomerListing.DB
{
    public interface IApplicationDbContext
    {
        DbSet<Customer> Customers { get; set; }
        Task<int> SaveChangesAsync();
    }
    public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Customer>().ToTable("Customers");

            builder.Entity<Customer>().Property(f => f.CustomerId).IsRequired();
            builder.Entity<Customer>().Property(f => f.FirstName).IsRequired();
            builder.Entity<Customer>().Property(f => f.LastName).IsRequired();
            builder.Entity<Customer>().Property(f => f.Cellphone).HasMaxLength(10);
        }
    }
}
