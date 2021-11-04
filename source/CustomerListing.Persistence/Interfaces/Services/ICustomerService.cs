using CustomerListing.DB.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerListing.Persistence.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetCustomersAsync(PaginationFilter paginationFilter = null);
        Task<bool> CreateCustomerAsync(Customer customer);
        Task<Customer> GetCustomerByIdAsync(Guid customerId);
        Task<bool> UpdateCustomerAsync(Customer customerToUpdate);
        Task<bool> DeleteCustomerAsync(Guid customerId);
    }
}
