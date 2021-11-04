using AutoMapper;
using CustomerListing.DB.Domain;
using CustomerListing.Persistence.V1.Requests;
using CustomerListing.Persistence.V1.Requests.Queries;

namespace CustomerListing.Server.MappingProfiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<PaginationQuery, PaginationFilter>().ReverseMap();
            CreateMap<CreateCustomerRequest, Customer>().ReverseMap();
            CreateMap<UpdateCustomerRequest, Customer>().ReverseMap();
        }
    }
}
