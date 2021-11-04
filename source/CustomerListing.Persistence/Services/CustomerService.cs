using CustomerListing.DB;
using CustomerListing.DB.Domain;
using CustomerListing.Persistence.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerListing.Persistence.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IApplicationDbContext _dataContext;

        public CustomerService(IApplicationDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Customer>> GetCustomersAsync(PaginationFilter paginationFilter = null)
        {
            var queryable = _dataContext.Customers.AsQueryable();

            if (paginationFilter == null)
            {
                return await queryable.ToListAsync();
            }

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable.Skip(skip).Take(paginationFilter.PageSize).ToListAsync();
        }

        public async Task<bool> CreateCustomerAsync(Customer customer)
        {
            var isSaved = false;
            try
            {
                _dataContext.Customers.Add(customer);
                isSaved = await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                return isSaved;
            }
            return isSaved;
        }

        public async Task<Customer> GetCustomerByIdAsync(Guid customerId)
        {
            return await _dataContext.Customers
               .SingleOrDefaultAsync(x => x.CustomerId == customerId);
        }

        public async Task<bool> UpdateCustomerAsync(Customer customerToUpdate)
        {
            _dataContext.Customers.Update(customerToUpdate);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCustomerAsync(Guid customerId)
        {
            var customer = await GetCustomerByIdAsync(customerId);

            if (customer == null)
                return false;

            _dataContext.Customers.Remove(customer);
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
