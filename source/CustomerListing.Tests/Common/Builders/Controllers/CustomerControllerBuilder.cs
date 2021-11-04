using AutoMapper;
using CustomerListing.Persistence.Interfaces.Services;
using CustomerListing.Server.Controllers.V1;
using NSubstitute;

namespace CustomerListing.Tests.Common.Builders.Controllers
{
    public class CustomerControllerBuilder
    {
        public CustomerControllerBuilder()
        {
            Mapper = Substitute.For<IMapper>();
            UriService = Substitute.For<IUriService>();
            CustomerService = Substitute.For<ICustomerService>();
        }

        public IMapper Mapper { get; private set; }
        public IUriService UriService { get; private set; }
        public ICustomerService CustomerService { get; private set; }

        public CustomerControllerBuilder WithMapper(IMapper mapper)
        {
            Mapper = mapper;
            return this;
        }

        public CustomerControllerBuilder WithUriService(IUriService uriService)
        {
            UriService = uriService;
            return this;
        }

        public CustomerControllerBuilder WithCustomerService(ICustomerService customerService)
        {
            CustomerService = customerService;
            return this;
        }

        public CustomerController Build()
        {
            return new CustomerController(CustomerService, Mapper, UriService);
        }
    }
}
