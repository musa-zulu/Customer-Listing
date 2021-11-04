using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CustomerListing.DB;
using CustomerListing.Persistence.Services;
using CustomerListing.Persistence.Interfaces.Services;

namespace CustomerListing.Server.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IConfigurationRoot configRoot)
        {            
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ?? configRoot["ConnectionStrings:DefaultConnection"]
            , b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();        
            services.AddScoped<ICustomerService, CustomerService>();        
        }
    }
}