using CustomerListing.Persistence.V1.Requests.Queries;
using System;

namespace CustomerListing.Persistence.Interfaces.Services
{
    public interface IUriService
    {
        Uri GetAllUri(PaginationQuery pagination = null);
        Uri GetCustomerUri(string customerId);
    }
}
