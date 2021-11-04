using AutoMapper;
using CustomerListing.DB.Domain;
using CustomerListing.Persistence.V1.Responses;

namespace CustomerListing.Server.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Customer, CustomerResponse>().ReverseMap();
        }
    }
}
