using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CustomerListing.DB;

namespace CustomerListing.Tests.Common.Helpers
{
    public class FakeDbContext
    {
        public FakeDbContext(string name)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(name)
                         .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                         .Options;

            DbContext = new ApplicationDbContext(options);
        }
        public ApplicationDbContext DbContext;

        public async Task Add(params object[] data)
        {
            DbContext.AddRange(data);
            await DbContext.SaveChangesAsync();
        }
    }
}
