using Microsoft.AspNetCore.WebUtilities;
using CustomerListing.Persistence.V1.Requests.Queries;
using System;
using CustomerListing.Persistence.Interfaces.Services;
using CustomerListing.Persistence.V1;

namespace CustomerListing.Persistence.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetAllUri(PaginationQuery pagination = null)
        {
            var uri = new Uri(_baseUri);

            if (pagination == null)
            {
                return uri;
            }

            var modifiedUri = QueryHelpers.AddQueryString(_baseUri, "pageNumber", pagination.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", pagination.PageSize.ToString());

            return new Uri(modifiedUri);

        }

        public Uri GetCustomerUri(string customerId)
        {
            return new Uri(_baseUri + ApiRoutes.Customers.Get.Replace("{customerId}", customerId));
        }
    }
}
