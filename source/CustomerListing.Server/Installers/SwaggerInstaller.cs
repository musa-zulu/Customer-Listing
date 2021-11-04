using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace CustomerListing.Server.Installers
{
    public class SwaggerInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IConfigurationRoot configRoot)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Customer Listing System API",
                    Version = "v1",
                    Contact = new OpenApiContact()
                    {
                        Email = "zuluchs@gmail.com",
                        Name = "Musa Zulu"                        
                    },
                    Description = "Developed by Musa Zulu, please click on the link below to contact him via email"
                });

                x.ExampleFilters();
            });

            services.AddSwaggerExamplesFromAssemblyOf<Startup>();
        }
    }
}