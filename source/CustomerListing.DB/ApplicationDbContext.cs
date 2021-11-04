using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CustomerListing.DB
{
    public interface IApplicationDbContext
    {
        Task<int> SaveChangesAsync();
    }
    public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }
        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
